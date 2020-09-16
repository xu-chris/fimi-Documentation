using System.Collections.Generic;
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

        private GameObject _gameObject;

        // Definition
        private readonly Dictionary<int, Joint> _joints;

        private readonly Dictionary<int, Bone> _bones;

        public Skeleton(int id)
        {
            this._id = id;
            
            _gameObject = new GameObject
            {
                name = "Skeleton_" + id
            };

            _bones = new Dictionary<int, Bone>
            {
                {0, new Bone(BoneType.LowerBody, 0, 1, SkeletonColor, _gameObject)},
                {1, new Bone(BoneType.UpperBody, 1, 2, SkeletonColor, _gameObject)},
                {2, new Bone(BoneType.Neck, 2, 3, SkeletonColor, _gameObject)},
                {3, new Bone(BoneType.Head, 3, 4, SkeletonColor, _gameObject)},

                // Left
                {4, new Bone(BoneType.LeftShoulder, 2, 5, SkeletonColor, _gameObject)},
                {5, new Bone(BoneType.LeftElbow, 5, 6, SkeletonColor, _gameObject)},
                {6, new Bone(BoneType.LeftForearm, 6, 7, SkeletonColor, _gameObject)},
                {7, new Bone(BoneType.LeftHand, 7, 8, SkeletonColor, _gameObject)},
                {8, new Bone(BoneType.LeftHip, 0, 9, SkeletonColor, _gameObject)},
                {9, new Bone(BoneType.LeftThigh, 9, 10, SkeletonColor, _gameObject)},
                {10, new Bone(BoneType.LeftLowerLeg, 10, 11, SkeletonColor, _gameObject)},
                {11, new Bone(BoneType.LeftFoot, 11, 12, SkeletonColor, _gameObject)},

                // Right
                {12, new Bone(BoneType.RightShoulder, 2, 13, SkeletonColor, _gameObject)},
                {13, new Bone(BoneType.RightElbow, 13, 14, SkeletonColor, _gameObject)},
                {14, new Bone(BoneType.RightForearm, 14, 15, SkeletonColor, _gameObject)},
                {15, new Bone(BoneType.RightHand, 15, 16, SkeletonColor, _gameObject)},
                {16, new Bone(BoneType.RightHip, 0, 17, SkeletonColor, _gameObject)},
                {17, new Bone(BoneType.RightThigh, 17, 18, SkeletonColor, _gameObject)},
                {18, new Bone(BoneType.RightLowerLeg, 18, 19, SkeletonColor, _gameObject)},
                {19, new Bone(BoneType.RightFoot, 19, 20, SkeletonColor, _gameObject)}
            };
        
            _joints = new Dictionary<int, Joint>
            {
                {0, new Joint(0, "Spine_1_RX", SkeletonColor, SphereRadius, _gameObject)},
                {1, new Joint(1, "Spine_2_RX", SkeletonColor, SphereRadius, _gameObject)},
                {2, new Joint(2, "Spine_3_RX", SkeletonColor, SphereRadius, _gameObject)},
                {3, new Joint(3, "Neck_1_RX", SkeletonColor, SphereRadius, _gameObject)},
                {4, new Joint(4, "Head_EE_RY", SkeletonColor, SphereRadius, _gameObject)},

                // Left
                {5, new Joint(5, "Left_Shoulder_RX", SkeletonColor, SphereRadius, _gameObject)},
                {6, new Joint(6, "Left_Elbow_RX", SkeletonColor, SphereRadius, _gameObject)},
                {7, new Joint(7, "Left_Hand_RX", SkeletonColor, SphereRadius, _gameObject)},
                {8, new Joint(8, "Left_Hand_EE_RX", SkeletonColor, SphereRadius, _gameObject)},
                {9, new Joint(9, "Left_Hip_RX", SkeletonColor, SphereRadius, _gameObject)},
                {10, new Joint(10, "Left_Knee_RX", SkeletonColor, SphereRadius, _gameObject)},
                {11, new Joint(11, "Left_Ankle_RX", SkeletonColor, SphereRadius, _gameObject)},
                {12, new Joint(12, "Left_Foot_EE", SkeletonColor, SphereRadius, _gameObject)},

                // Right
                {13, new Joint(13, "Right_Shoulder_RX", SkeletonColor, SphereRadius, _gameObject)},
                {14, new Joint(14, "Right_Elbow_RX", SkeletonColor, SphereRadius, _gameObject)},
                {15, new Joint(15, "Right_Hand_RX", SkeletonColor, SphereRadius, _gameObject)},
                {16, new Joint(16, "Right_Hand_EE_RX", SkeletonColor, SphereRadius, _gameObject)},
                {17, new Joint(17, "Right_Hip_RX", SkeletonColor, SphereRadius, _gameObject)},
                {18, new Joint(18, "Right_Knee_RX", SkeletonColor, SphereRadius, _gameObject)},
                {19, new Joint(19, "Right_Ankle_RX", SkeletonColor, SphereRadius, _gameObject)},
                {20, new Joint(20, "Right_Foot_EE", SkeletonColor, SphereRadius, _gameObject)}
            };
        }

        public void SetIsVisible(bool visibility)
        {
            _gameObject.SetActive(visibility);
            // for (var i = 0; i < _joints.Count; ++i) _joints[i].SetIsVisible(visibility);
            //
            // for (var i = 0; i < _bones.Count; i++) _bones[i].SetIsVisible(visibility);
        }

        public void SetSkeleton(Vector3[] joints, GameObject plane, float lowestY)
        {
            var planeFootBuffer = 0.02f;
            var moveAmount = plane.transform.position.y;
            var differenceWithPlaneAndFeet = lowestY - moveAmount - planeFootBuffer;

            UpdateJoints(joints, differenceWithPlaneAndFeet);
            UpdateBones(joints, differenceWithPlaneAndFeet);
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
                var bone = _bones[i];
                bone.SetBoneSizeAndPosition(joints[bone.JointA], joints[bone.JointB], differenceWithPlaneAndFeet);
            }
        }
    }
}