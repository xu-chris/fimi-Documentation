using System.Collections.Generic;
using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts
{
    public class Skeleton
    {
        // Parameters
        private int _id;

        // Skeleton design
        private static readonly Color SkeletonColor = Color.black;
        private static readonly float SphereRadius = 0.05f;

        private readonly GameObject _gameObject;

        // Definition
        private readonly List<Joint> _joints;

        private readonly List<Bone> _bones;

        public Skeleton(int id, bool withGameObjects = true)
        {
            this._id = id;
        
            _gameObject = new GameObject
            {
                name = "Skeleton_" + id
            };

            _bones = new List<Bone> {
                new Bone(BoneType.LowerBody, 0, 1, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.UpperBody, 1, 2, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.Neck, 2, 3, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.Head, 3, 4, SkeletonColor, _gameObject, withGameObjects),

                // Left
                new Bone(BoneType.LeftShoulder, 2, 5, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftElbow, 5, 6, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftForearm, 6, 7, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftHand, 7, 8, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftHip, 0, 9, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftThigh, 9, 10, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftLowerLeg, 10, 11, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.LeftFoot, 11, 12, SkeletonColor, _gameObject, withGameObjects),

                // Right
                new Bone(BoneType.RightShoulder, 2, 13, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightElbow, 13, 14, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightForearm, 14, 15, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightHand, 15, 16, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightHip, 0, 17, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightThigh, 17, 18, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightLowerLeg, 18, 19, SkeletonColor, _gameObject, withGameObjects),
                new Bone(BoneType.RightFoot, 19, 20, SkeletonColor, _gameObject, withGameObjects)
            };
    
            _joints = new List<Joint> {
                new Joint(0, JointType.Spine1Rx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(1, JointType.Spine2Rx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(2, JointType.Spine3Rx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(3, JointType.Neck1Rx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(4, JointType.HeadEeRy, SkeletonColor, SphereRadius, _gameObject, withGameObjects),

                // Left
                new Joint(5, JointType.LeftShoulderRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(6, JointType.LeftElbowRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(7, JointType.LeftHandRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(8, JointType.LeftHandEeRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(9, JointType.LeftHipRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(10, JointType.LeftKneeRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(11, JointType.LeftAnkleRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(12, JointType.LeftFootEe, SkeletonColor, SphereRadius, _gameObject, withGameObjects),

                // Right
                new Joint(13, JointType.RightShoulderRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(14, JointType.RightElbowRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(15, JointType.RightHandRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(16, JointType.RightHandEeRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(17, JointType.RightHipRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(18, JointType.RightKneeRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(19, JointType.RightAnkleRx, SkeletonColor, SphereRadius, _gameObject, withGameObjects),
                new Joint(20, JointType.RightFootEe, SkeletonColor, SphereRadius, _gameObject, withGameObjects)
            };
        }

        public void SetIsVisible(bool visibility)
        {
            _gameObject.SetActive(visibility);
        }

        public void SetSkeleton(Vector3[] joints, GameObject plane, float lowestY)
        {
            var planeFootBuffer = 0.02f;
            var moveAmount = plane.transform.position.y;
            var differenceWithPlaneAndFeet = lowestY - moveAmount - planeFootBuffer;

            UpdateJoints(joints, differenceWithPlaneAndFeet);
            UpdateBones(joints, differenceWithPlaneAndFeet);
            
            CheckAngleBetweenBones(BoneType.LeftLowerLeg, BoneType.LeftThigh, 90, 5);
            CheckAngleBetweenBones(BoneType.RightLowerLeg, BoneType.RightThigh, 90, 5);
            
            CheckAngleBetweenBones(BoneType.LeftForearm, BoneType.LeftElbow, 0, 5);
            CheckAngleBetweenBones(BoneType.RightForearm, BoneType.RightElbow, 0, 5);
        }

        private void UpdateJoints(Vector3[] joints, float differenceWithPlaneAndFeet)
        {
            for (var i = 0; i < _joints.Count; i++)
            {
                var vector = new Vector3(joints[i][0], joints[i][1] - differenceWithPlaneAndFeet, joints[i][2]);
                _joints[i].SetJointPosition(vector);
            }
        }

        private void UpdateBones(Vector3[] joints, float differenceWithPlaneAndFeet)
        {
            for (var i = 0; i < _bones.Count; i++)
            {
                var startJoint = joints[_bones[i].JointIndexA];
                var endJoint = joints[_bones[i].JointIndexB];
                _bones[i] = _bones[i].SetBoneSizeAndPosition(startJoint, endJoint, differenceWithPlaneAndFeet);
            }
        }

        private void CheckAngleBetweenBones(BoneType boneTypeA, BoneType boneTypeB, float expectedAngle, float tolerance)
        {
            // Fetching bones
            var boneA = _bones.Find(item => item.BoneType.Equals(boneTypeA));
            var boneB = _bones.Find(item => item.BoneType.Equals(boneTypeB));

            // Color bones accordingly
            if (IsBonesInDegreeRange(expectedAngle, tolerance, boneA, boneB))
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

        public static bool IsBonesInDegreeRange(float expectedAngle, float tolerance, Bone boneA, Bone boneB)
        {
            var calculatedAngle = Vector3.Angle(boneA.BoneVector, boneB.BoneVector);
            Debug.Log("Calculated angle: " + calculatedAngle + ", expected angle is between " + (expectedAngle - tolerance) + " and " + (expectedAngle + tolerance));
            return !(calculatedAngle > expectedAngle + tolerance) && !(calculatedAngle < expectedAngle - tolerance);
        }
    }
}