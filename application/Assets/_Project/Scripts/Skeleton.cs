using System.Collections.Generic;
using UnityEngine;

public class Skeleton
{
    // Skeleton design
    private static readonly Color SkeletonColor = Color.black;
    private static readonly float SphereRadius = 0.05f;

    // Definition
    private static readonly Dictionary<int, Joint> Joints = new Dictionary<int, Joint>
    {
        {0, new Joint(0, "Spine_1_RX", SkeletonColor, SphereRadius)},
        {1, new Joint(1, "Spine_2_RX", SkeletonColor, SphereRadius)},
        {2, new Joint(2, "Spine_3_RX", SkeletonColor, SphereRadius)},
        {3, new Joint(3, "Neck_1_RX", SkeletonColor, SphereRadius)},
        {4, new Joint(4, "Head_EE_RY", SkeletonColor, SphereRadius)},

        // Left
        {5, new Joint(5, "Left_Shoulder_RX", SkeletonColor, SphereRadius)},
        {6, new Joint(6, "Left_Elbow_RX", SkeletonColor, SphereRadius)},
        {7, new Joint(7, "Left_Hand_RX", SkeletonColor, SphereRadius)},
        {8, new Joint(8, "Left_Hand_EE_RX", SkeletonColor, SphereRadius)},
        {9, new Joint(9, "Left_Hip_RX", SkeletonColor, SphereRadius)},
        {10, new Joint(10, "Left_Knee_RX", SkeletonColor, SphereRadius)},
        {11, new Joint(11, "Left_Ankle_RX", SkeletonColor, SphereRadius)},
        {12, new Joint(12, "Left_Foot_EE", SkeletonColor, SphereRadius)},

        // Right
        {13, new Joint(13, "Right_Shoulder_RX", SkeletonColor, SphereRadius)},
        {14, new Joint(14, "Right_Elbow_RX", SkeletonColor, SphereRadius)},
        {15, new Joint(15, "Right_Hand_RX", SkeletonColor, SphereRadius)},
        {16, new Joint(16, "Right_Hand_EE_RX", SkeletonColor, SphereRadius)},
        {17, new Joint(17, "Right_Hip_RX", SkeletonColor, SphereRadius)},
        {18, new Joint(18, "Right_Knee_RX", SkeletonColor, SphereRadius)},
        {19, new Joint(19, "Right_Ankle_RX", SkeletonColor, SphereRadius)},
        {20, new Joint(20, "Right_Foot_EE", SkeletonColor, SphereRadius)}
    };

    private static readonly Dictionary<int, Bone> Bones = new Dictionary<int, Bone>
    {
        {0, new Bone(BoneType.LowerBody, 0, 1, SkeletonColor)},
        {1, new Bone(BoneType.UpperBody, 1, 2, SkeletonColor)},
        {2, new Bone(BoneType.Neck, 2, 3, SkeletonColor)},
        {3, new Bone(BoneType.Head, 3, 4, SkeletonColor)},

        // Left
        {4, new Bone(BoneType.LeftShoulder, 2, 5, SkeletonColor)},
        {5, new Bone(BoneType.LeftElbow, 5, 6, SkeletonColor)},
        {6, new Bone(BoneType.LeftForearm, 6, 7, SkeletonColor)},
        {7, new Bone(BoneType.LeftHand, 7, 8, SkeletonColor)},
        {8, new Bone(BoneType.LeftHip, 0, 9, SkeletonColor)},
        {9, new Bone(BoneType.LeftThigh, 9, 10, SkeletonColor)},
        {10, new Bone(BoneType.LeftLowerLeg, 10, 11, SkeletonColor)},
        {11, new Bone(BoneType.LeftFoot, 11, 12, SkeletonColor)},

        // Right
        {12, new Bone(BoneType.RightShoulder, 2, 13, SkeletonColor)},
        {13, new Bone(BoneType.RightElbow, 13, 14, SkeletonColor)},
        {14, new Bone(BoneType.RightForearm, 14, 15, SkeletonColor)},
        {15, new Bone(BoneType.RightHand, 15, 16, SkeletonColor)},
        {16, new Bone(BoneType.RightHip, 0, 17, SkeletonColor)},
        {17, new Bone(BoneType.RightThigh, 17, 18, SkeletonColor)},
        {18, new Bone(BoneType.RightLowerLeg, 18, 19, SkeletonColor)},
        {19, new Bone(BoneType.RightFoot, 19, 20, SkeletonColor)}
    };

    // private GameObject _feet;
    //
    // private GameObject _lFoot;
    // private GameObject _rFoot;

    public Skeleton()
    {
        // TODO: Check if feed are still necessary
        // SetupFeet();
    }

    // private void SetupFeet()
    // {
    //     _feet = Object.Instantiate(GameObject.Find("feet"));
    //     _lFoot = GameObject.Find(_feet.name + "/Mannequin_Amature/Foot_L");
    //     _rFoot = GameObject.Find(_feet.name + "/Mannequin_Amature/Foot_R");
    //     GameObject.Find("feet").SetActive(false);
    // }

    public void SetIsVisible(bool visibility)
    {
        // _feet.SetActive(visibility);
        for (var i = 0; i < Joints.Count; ++i) Joints[i].SetIsVisible(visibility);

        for (var i = 0; i < Bones.Count; i++) Bones[i].SetIsVisible(visibility);
    }

    public void SetSkeleton(Vector3[] joints, GameObject plane, float lowestY)
    {
        var planeFootBuffer = 0.02f;
        var moveAmount = plane.transform.position.y;
        var differenceWithPlaneAndFeet = lowestY - moveAmount - planeFootBuffer;

        UpdateJoints(joints, differenceWithPlaneAndFeet);
        UpdateBones(joints, differenceWithPlaneAndFeet);
        // UpdateFeet(joints, differenceWithPlaneAndFeet);
    }

    // private void UpdateFeet(Vector3[] joints, float differenceWithPlaneAndFeet)
    // {
    //     // Draw mesh 21 20
    //     _rFoot.transform.rotation = Quaternion.LookRotation((joints[20] - joints[19]).normalized);
    //     _rFoot.transform.rotation = Quaternion.Euler(_rFoot.transform.eulerAngles + new Vector3(140, 0, 0));
    //     _rFoot.transform.position = joints[19] - new Vector3(0, differenceWithPlaneAndFeet, 0);
    //
    //     // Rotate z-axis to align with bone vector 12 11
    //     _lFoot.transform.rotation = Quaternion.LookRotation((joints[12] - joints[11]).normalized);
    //     _lFoot.transform.rotation = Quaternion.Euler(_lFoot.transform.eulerAngles + new Vector3(140, 0, 0));
    //     // Position at middle
    //     _lFoot.transform.position = joints[11] - new Vector3(0, differenceWithPlaneAndFeet, 0);
    // }

    private void UpdateJoints(Vector3[] joints, float differenceWithPlaneAndFeet)
    {
        for (var i = 0; i < Joints.Count; ++i)
        {
            var vector = new Vector3(joints[i][0], joints[i][1] - differenceWithPlaneAndFeet, joints[i][2]);
            Joints[i].SetJointPosition(vector);
        }
    }

    private void UpdateBones(Vector3[] joints, float differenceWithPlaneAndFeet)
    {
        for (var i = 0; i < Bones.Count; i++)
        {
            var bone = Bones[i];
            bone.SetBoneSizeAndPosition(joints[bone.JointA], joints[bone.JointB], differenceWithPlaneAndFeet);
        }
    }
}