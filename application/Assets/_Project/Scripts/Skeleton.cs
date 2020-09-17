using System;
using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.DomainValues;
using UnityEngine;
using UnityEngine.Assertions;
using YamlDotNet.Serialization.NamingConventions;
using Joint = _Project.Scripts.Joint;

namespace _Project.Scripts
{
    public class Skeleton
    {
        // Parameters
        private int id;

        // Skeleton design
        private static readonly Color skeletonColor = Color.black;
        private static readonly float sphereRadius = 0.05f;

        private readonly GameObject gameObject;

        // Definition
        private readonly List<Joint> joints;

        private readonly List<Bone> bones;

        public Skeleton(int id, bool withGameObjects = true)
        {
            this.id = id;
        
            gameObject = new GameObject
            {
                name = "Skeleton_" + id
            };

            bones = new List<Bone> {
                new Bone(BoneType.LOWER_BODY, 0, 1, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.UPPER_BODY, 1, 2, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.NECK, 2, 3, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.HEAD, 3, 4, skeletonColor, gameObject, withGameObjects),

                // Left
                new Bone(BoneType.LEFT_SHOULDER, 2, 5, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_ELBOW, 5, 6, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_FOREARM, 6, 7, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_HAND, 7, 8, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_HIP, 0, 9, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_THIGH, 9, 10, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_LOWER_LEG, 10, 11, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.LEFT_FOOT, 11, 12, skeletonColor, gameObject, withGameObjects),

                // Right
                new Bone(BoneType.RIGHT_SHOULDER, 2, 13, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_ELBOW, 13, 14, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_FOREARM, 14, 15, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_HAND, 15, 16, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_HIP, 0, 17, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_THIGH, 17, 18, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_LOWER_LEG, 18, 19, skeletonColor, gameObject, withGameObjects),
                new Bone(BoneType.RIGHT_FOOT, 19, 20, skeletonColor, gameObject, withGameObjects)
            };
    
            joints = new List<Joint> {
                new Joint(0, JointType.SPINE1_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(1, JointType.SPINE2_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(2, JointType.SPINE3_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(3, JointType.NECK1_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(4, JointType.HEAD_EE_RY, skeletonColor, sphereRadius, gameObject, withGameObjects),

                // Left
                new Joint(5, JointType.LEFT_SHOULDER_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(6, JointType.LEFT_ELBOW_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(7, JointType.LEFT_HAND_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(8, JointType.LEFT_HAND_EE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(9, JointType.LEFT_HIP_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(10, JointType.LEFT_KNEE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(11, JointType.LEFT_ANKLE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(12, JointType.LEFT_FOOT_EE, skeletonColor, sphereRadius, gameObject, withGameObjects),

                // Right
                new Joint(13, JointType.RIGHT_SHOULDER_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(14, JointType.RIGHT_ELBOW_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(15, JointType.RIGHT_HAND_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(16, JointType.RIGHT_HAND_EE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(17, JointType.RIGHT_HIP_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(18, JointType.RIGHT_KNEE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(19, JointType.RIGHT_ANKLE_RX, skeletonColor, sphereRadius, gameObject, withGameObjects),
                new Joint(20, JointType.RIGHT_FOOT_EE, skeletonColor, sphereRadius, gameObject, withGameObjects)
            };
        }

        public void SetIsVisible(bool visibility)
        {
            gameObject.SetActive(visibility);
        }

        public void SetSkeleton(Vector3[] jointEstimation, GameObject plane, float lowestY)
        {
            var planeFootBuffer = 0.02f;
            var moveAmount = plane.transform.position.y;
            var differenceWithPlaneAndFeet = lowestY - moveAmount - planeFootBuffer;

            UpdateJoints(jointEstimation, differenceWithPlaneAndFeet);
            UpdateBones(jointEstimation, differenceWithPlaneAndFeet);
        }

        public void CheckRules(List<Rule> rules)
        {
            foreach (var rule in rules)
            {
                switch (rule.type.ToRuleType())
                {
                    case RuleType.ANGLE:
                        CheckAngleBetweenBones(rule.bones[0].ToBoneType(), rule.bones[1].ToBoneType(), rule.angleDefinition);
                        break;
                    case RuleType.ANGLE_THRESHOLD:
                        CheckAngleBetweenBonesIsNotOverThreshold(rule.bones[0].ToBoneType(), rule.bones[1].ToBoneType(), rule.angleDefinition);
                        break;
                }
            }
        }

        private void UpdateJoints(Vector3[] jointEstimation, float differenceWithPlaneAndFeet)
        {
            for (var i = 0; i < this.joints.Count; i++)
            {
                var vector = new Vector3(jointEstimation[i][0], jointEstimation[i][1] - differenceWithPlaneAndFeet, jointEstimation[i][2]);
                this.joints[i].SetJointPosition(vector);
            }
        }

        private void UpdateBones(Vector3[] jointEstimation, float differenceWithPlaneAndFeet)
        {
            foreach (var bone in bones)
            {
                var startJoint = jointEstimation[bone.jointIndexA];
                var endJoint = jointEstimation[bone.jointIndexB];
                bone.SetBoneSizeAndPosition(startJoint, endJoint, differenceWithPlaneAndFeet);
            }
        }

        private void CheckAngleBetweenBones(BoneType boneTypeA, BoneType boneTypeB, AngleDefinition angleDefinition)
        {
            // Fetching bones
            var boneA = GetBone(boneTypeA);
            var boneB = GetBone(boneTypeB);

            // Color bones accordingly
            if (IsBonesInDegreeRange(angleDefinition.expected, angleDefinition.lowerTolerance, angleDefinition.higherTolerance, boneA, boneB))
            {
                boneA.Colorize(Color.green);
                boneB.Colorize(Color.green);
            }
            else
            {
                boneA.Colorize(Color.red);
                boneB.Colorize(Color.red);
            }
        }

        private void CheckAngleBetweenBonesIsNotOverThreshold(BoneType boneTypeA, BoneType boneTypeB,
            AngleDefinition angleDefinition)
        {
            CheckAngleBetweenBonesIsNotOverThreshold(boneTypeA, boneTypeB, angleDefinition.expected, angleDefinition.threshold, angleDefinition.lowerTolerance, angleDefinition.higherTolerance);
        }

        private void CheckAngleBetweenBonesIsNotOverThreshold(BoneType boneTypeA, BoneType boneTypeB,
            float expectedAngle, float threshold, float lowerTolerance = 0f, float higherTolerance = 0f)
        {
            Assert.IsTrue(
                (threshold > expectedAngle && higherTolerance + expectedAngle <= threshold) ||
                (threshold < expectedAngle && expectedAngle - lowerTolerance >= threshold)
            );

            // Fetching bones
            var boneA = GetBone(boneTypeA);
            var boneB = GetBone(boneTypeB);

            // Color bones accordingly
            if (IsBonesInDegreeRange(expectedAngle, lowerTolerance, higherTolerance, boneA, boneB))
            {
                boneA.Colorize(Color.green);
                boneB.Colorize(Color.green);
            }
            else if (!IsBonesInAngleThreshold(threshold, boneA, boneB, expectedAngle))
            {
                boneA.Colorize(Color.red);
                boneB.Colorize(Color.red);
            }
            else
            {
                boneA.Colorize(skeletonColor);
                boneB.Colorize(skeletonColor);
            }
        }

        public static bool IsBonesInDegreeRange(float expectedAngle, float lowerTolerance, float higherTolerance, Bone boneA, Bone boneB)
        {
            var calculatedAngle = GetAngleBetweenBones(boneA, boneB);
            Debug.Log("Calculated angle: " + calculatedAngle + ", expected angle is between " + (expectedAngle - lowerTolerance) + " and " + (expectedAngle + higherTolerance));
            return !(calculatedAngle > expectedAngle + higherTolerance) && !(calculatedAngle < expectedAngle - lowerTolerance);
        }

        /**
         * Checks if the angle is in given threshold.
         * Returns true if
         * - the threshold is lower than expectedAngle AND the calculated angle is higher than the threshold, OR
         * - the threshold is higher than expectedAngle AND the calculated angle is lower than the threshold
         */
        private static bool IsBonesInAngleThreshold(float threshold, Bone boneA, Bone boneB, float expectedAngle)
        {
            var calculatedAngle = GetAngleBetweenBones(boneA, boneB);
            Debug.Log("Calculated angle: " + calculatedAngle + ", threshold is: " + threshold + " degree");
            return expectedAngle < threshold ? (calculatedAngle <= threshold) : (calculatedAngle >= threshold);
        }
        
        /// <summary>Returns the angle between two bones.
        /// Calculated by the inverse Cosinus of the dot product of both, divided by the vector magnitude of both bones.
        /// The angle returned is the unsigned angle between the two vectors, so the smaller of possible angles between the vectors is used.
        /// The result is never greater than 180 degrees.</summary>
        /// 
        /// <param name="boneA">The first bone</param>
        /// <param name="boneB">The second bone</param>
        private static float GetAngleBetweenBones(Bone boneA, Bone boneB)
        {
            return Vector3.Angle(boneA.boneVector, boneB.boneVector);
        }

        /**
         * Returns the bone for a given BoneType.
         * @return Bone the bone.
         */
        private Bone GetBone(BoneType boneType)
        {
            return bones.Find(item => item.boneType.Equals(boneType));
        }
    }
}