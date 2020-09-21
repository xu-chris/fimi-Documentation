using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts
{
    public class Skeleton
    {
        // Skeleton design
        protected static readonly Color skeletonColor = Color.black;
        private static readonly float sphereRadius = 0.05f;

        private readonly List<Bone> bones;

        private readonly GameObject gameObject;

        // Definition
        private readonly List<Joint> joints;

        public Skeleton(int id, bool withGameObjects = true)
        {
            gameObject = new GameObject
            {
                name = "Skeleton_" + id
            };

            bones = new List<Bone>
            {
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

            joints = new List<Joint>
            {
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

        public void SetSkeleton(Vector3[] jointEstimation)
        {
            UpdateJoints(jointEstimation);
            UpdateBones(jointEstimation);
        }

        private void UpdateJoints(Vector3[] jointEstimation)
        {
            for (var i = 0; i < joints.Count; i++)
            {
                var vector = new Vector3(jointEstimation[i][0], jointEstimation[i][1], jointEstimation[i][2]);
                joints[i].SetJointPosition(vector);
            }
        }

        private void UpdateBones(Vector3[] jointEstimation)
        {
            foreach (var bone in bones)
            {
                var startJoint = jointEstimation[bone.jointIndexA];
                var endJoint = jointEstimation[bone.jointIndexB];
                bone.SetBoneSizeAndPosition(startJoint, endJoint);
            }
        }

        /**
         * Returns the bone for a given BoneType.
         * @return Bone the bone.
         */
        protected Bone GetBone(BoneType boneType)
        {
            return bones.Find(item => item.boneType.Equals(boneType));
        }
    }
}