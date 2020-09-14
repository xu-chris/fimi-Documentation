using System.Collections.Generic;
using UnityEngine;

public class RunLiveXNect : RunLive
{
    private readonly Color _skeletonColor = Color.black;
    private GameObject _mBall;
    private Vector3 _originalBallPos;
    private List<int> _validJointIdx;

    public override void Start()
    {
        // Print joints: 21
        //
        //{ "Spine_1_RX", "Spine_2_RX","Spine_3_RX","Neck_1_RX","Head_EE_RY"};
        //{ "Shoulder_RX","Elbow_RX","Hand_RX","_EE_RX", "Hip_RX","Knee_RX","Ankle_RX","Foot_EE" }; L + R

        var jSize = 21;

        _mBall = GameObject.Find("Ball");
        _mBall.GetComponent<Renderer>().enabled = false;

        _originalBallPos = new Vector3(_mBall.transform.position.x, _mBall.transform.position.y,
            _mBall.transform.position.z);

        LFoot = new GameObject[m_totalNumPeople];
        RFoot = new GameObject[m_totalNumPeople];
        m_Feet = new GameObject[m_totalNumPeople];

        for (var p = 0; p < m_totalNumPeople; ++p)
        {
            m_Feet[p] = Object.Instantiate(GameObject.Find("feet"));
            m_Feet[p].name = m_Feet[p].name + "_" + p;

            LFoot[p] = GameObject.Find(m_Feet[p].name + "/Mannequin_Amature/Foot_L");
            RFoot[p] = GameObject.Find(m_Feet[p].name + "/Mannequin_Amature/Foot_R");
        }

        GameObject.Find("feet").SetActive(false);
        m_JointSpheres = new GameObject[jSize, m_totalNumPeople];

        for (var p = 0; p < m_totalNumPeople; ++p)
        for (var i = 0; i < jSize; ++i)
        {
            m_JointSpheres[i, p] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_JointSpheres[i, p].GetComponent<Renderer>().material.color = _skeletonColor;

            // Size of spheres
            var SphereRadius = 0.05f;
            m_JointSpheres[i, p].transform.localScale = new Vector3(SphereRadius, SphereRadius, SphereRadius);
        }

        // Next up create ellipsoids for bones
        var nBones = 18;
        m_Bones = new GameObject[nBones, m_totalNumPeople];
        for (var p = 0; p < m_totalNumPeople; ++p)
        for (var i = 0; i < nBones; ++i)
        {
            m_Bones[i, p] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_Bones[i, p].GetComponent<Renderer>().material.color = _skeletonColor;
        }
    }

    private void DrawSkeleton(int p, Vector3[] joints, float lowestY)
    {
        // Make floor stick to bottom-most joint (at index 16 or 20)
        var plane = GameObject.Find("CheckerboardPlane");
        var planeFootBuffer = 0.02f;
        var moveAmount = 0.0f;
        var differenceWithPlaneAndFeet = 0.0f;
        if (plane != null)
        {
            var origPos = plane.transform.position;

            moveAmount = plane.transform.position.y;

            differenceWithPlaneAndFeet = lowestY - moveAmount - planeFootBuffer;

            for (var i = 0; i < m_JointSpheres.GetLength(0); ++i)
            {
                var orig = joints[i];
                m_JointSpheres[i, p].transform.position =
                    new Vector3(orig[0], orig[1] - differenceWithPlaneAndFeet, orig[2]);
            }
        }

        //1-spine1 spine 2
        DrawEllipsoid(joints[0], joints[1], m_Bones[0, p], differenceWithPlaneAndFeet);
        //2-spine2 spine 3
        DrawEllipsoid(joints[1], joints[2], m_Bones[1, p], differenceWithPlaneAndFeet);
        //3-spine 3 neck 1                   
        DrawEllipsoid(joints[2], joints[3], m_Bones[2, p], differenceWithPlaneAndFeet);
        //4- neck 1 - head_ee                
        DrawEllipsoid(joints[3], joints[4], m_Bones[3, p], differenceWithPlaneAndFeet);

        //2-spine_3_rx, 8-left_shoulder_ry
        DrawEllipsoid(joints[2], joints[5], m_Bones[4, p], differenceWithPlaneAndFeet);
        //8-left_shoulder_ry, 9-left_elbow_rx
        DrawEllipsoid(joints[5], joints[6], m_Bones[5, p], differenceWithPlaneAndFeet);
        //9-left_elbow_rx, 10-Hand_RX
        DrawEllipsoid(joints[6], joints[7], m_Bones[6, p], differenceWithPlaneAndFeet);
        //10-Hand_RX, 11-hand ee
        DrawEllipsoid(joints[7], joints[8], m_Bones[7, p], differenceWithPlaneAndFeet);

        //2-spine_3_rx, 8-right_shoulder_ry
        DrawEllipsoid(joints[2], joints[13], m_Bones[8, p], differenceWithPlaneAndFeet);
        //8-left_shoulder_ry, 9-left_elbow_rx
        DrawEllipsoid(joints[13], joints[14], m_Bones[9, p], differenceWithPlaneAndFeet);
        //9-left_elbow_rx, 10-Hand_RX
        DrawEllipsoid(joints[14], joints[15], m_Bones[10, p], differenceWithPlaneAndFeet);
        //10-Hand_RX, 11-hand ee
        DrawEllipsoid(joints[15], joints[16], m_Bones[11, p], differenceWithPlaneAndFeet);


        //4-spine_1_rx, 19-left_hip_ry
        DrawEllipsoid(joints[0], joints[9], m_Bones[12, p], differenceWithPlaneAndFeet);
        //19-left_hip_ry, 20-left_knee_rx
        DrawEllipsoid(joints[9], joints[10], m_Bones[13, p], differenceWithPlaneAndFeet);
        //20-left_knee_rx, 21-left_ankle_ry
        DrawEllipsoid(joints[10], joints[11], m_Bones[14, p], differenceWithPlaneAndFeet);

        //4-spine_1_rx, 19-right_hip_ry
        DrawEllipsoid(joints[0], joints[17], m_Bones[15, p], differenceWithPlaneAndFeet);
        //19-left_hip_ry, 20-right_knee_rx
        DrawEllipsoid(joints[17], joints[18], m_Bones[16, p], differenceWithPlaneAndFeet);
        //20-left_knee_rx, 21-right_ankle_ry
        DrawEllipsoid(joints[18], joints[19], m_Bones[17, p], differenceWithPlaneAndFeet);


        // Disable toe sphere
        m_JointSpheres[12, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[20, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[11, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[19, p].GetComponent<MeshRenderer>().enabled = false;


        // Draw mesh 21 20
        RFoot[p].transform.rotation = Quaternion.LookRotation((joints[20] - joints[19]).normalized);
        RFoot[p].transform.rotation = Quaternion.Euler(RFoot[p].transform.eulerAngles + new Vector3(140, 0, 0));
        RFoot[p].transform.position = joints[19] - new Vector3(0, differenceWithPlaneAndFeet, 0);

        // Rotate z-axis to align with bone vector 12 11
        LFoot[p].transform.rotation = Quaternion.LookRotation((joints[12] - joints[11]).normalized);
        LFoot[p].transform.rotation = Quaternion.Euler(LFoot[p].transform.eulerAngles + new Vector3(140, 0, 0));
        // Position at middle
        LFoot[p].transform.position = joints[11] - new Vector3(0, differenceWithPlaneAndFeet, 0);
    }

    public override void Update(string line)
    {
        //Debug.Log(Line);
        if (Input.GetKey(KeyCode.B)) //reset ball
        {
            _mBall.transform.position = _originalBallPos;
            _mBall.transform.rotation = Quaternion.identity;
            _mBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _mBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.H)) //reset ball
            _mBall.GetComponent<Renderer>().enabled = false;

        if (Input.GetKey(KeyCode.S)) //reset ball
            _mBall.GetComponent<Renderer>().enabled = true;

        if (line.Length == 0)
            return;

        // Parse line
        var parseOffset = 0; // No offset for real-time system
        var numBones = m_Bones.GetLength(0);
        var tokens = line.Split(',');
        Debug.Log("Detected " + (tokens.Length - parseOffset) / 3 + " joints.");

        var num_joints = 22;

        if ((tokens.Length - parseOffset) / 3 % num_joints != 0)
            return;

        var joints = new Vector3[(tokens.Length - parseOffset) / 3];

        var currentNumPeople = (tokens.Length - parseOffset) / 3 / num_joints;
        Debug.Log("current_num_people " + currentNumPeople);
        var lowestY = 999.0f;

        for (var p = 0; p < currentNumPeople; ++p)
        {
            m_Feet[p].SetActive(true);

            for (var i = 0; i < m_JointSpheres.GetLength(0); ++i)
            {
                m_JointSpheres[i, p].SetActive(true);
                m_JointSpheres[i, p].GetComponent<Renderer>().material.color = _skeletonColor * .5f;

                if (i < numBones)
                {
                    m_Bones[i, p].SetActive(true);
                    m_Bones[i, p].GetComponent<Renderer>().material.color = _skeletonColor;
                }

                joints[i].x =
                    float.Parse(tokens[3 * p * num_joints + 3 * (i + 1) + 0 + parseOffset]) *
                    0.001f; // Mirror for ease of viewing (-1), no mirror (1)
                joints[i].y = float.Parse(tokens[3 * p * num_joints + 3 * (i + 1) + 1 + parseOffset]) * 0.001f;
                joints[i].z =
                    -float.Parse(tokens[3 * p * num_joints + 3 * (i + 1) + 2 + parseOffset]) *
                    0.001f; // Flip for Google VR

                if (joints[i].y < lowestY)
                    lowestY = joints[i].y;

                DrawSkeleton(p, joints, lowestY);
            }
        }

        for (var p = currentNumPeople; p < m_totalNumPeople; ++p)
        {
            m_Feet[p].SetActive(false);
            for (var i = 0; i < m_JointSpheres.GetLength(0); ++i)
            {
                m_JointSpheres[i, p].SetActive(false);

                if (i < numBones)
                    m_Bones[i, p].SetActive(false);
            }
        }
    }
}