using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class runLiveVNect : runLive
{
    private List<int> m_ValidJointIdx;
    private Vector3 originalBallPos;
    private GameObject m_Ball;
    private Color skeletonColor = Color.black;

    override public void Start()
    {
        m_isMoveFloor = true;


        // Print joints: 21
        //
       //{ "Spine_1_RX", "Spine_2_RX","Spine_3_RX","Neck_1_RX","Head_EE_RY"};
       //{ "Shoulder_RX","Elbow_RX","Hand_RX","_EE_RX", "Hip_RX","Knee_RX","Ankle_RX","Foot_EE" }; L + R

        int JSize = 21;
 
        m_Ball = GameObject.Find("Ball");
        m_Ball.GetComponent<Renderer>().enabled = false;

        originalBallPos = new Vector3(m_Ball.transform.position.x, m_Ball.transform.position.y, m_Ball.transform.position.z);
  

        LFoot = new GameObject[m_totalNumPeople];
        RFoot = new GameObject[m_totalNumPeople];
        m_Feet = new GameObject[m_totalNumPeople];



        for (int p = 0; p < m_totalNumPeople; ++p)
        {
            m_Feet[p] = GameObject.Instantiate(GameObject.Find("feet"));
            m_Feet[p].name = m_Feet[p].name + "_" + p.ToString();

            LFoot[p] = GameObject.Find(m_Feet[p].name + "/Mannequin_Amature/Foot_L");
            RFoot[p] = GameObject.Find(m_Feet[p].name + "/Mannequin_Amature/Foot_R");
        }
        GameObject.Find("feet").SetActive(false);
        m_JointSpheres = new GameObject[JSize, m_totalNumPeople];

        for (int p = 0; p < m_totalNumPeople; ++p)
            for (int i = 0; i < JSize; ++i)
        {
            m_JointSpheres[i,p] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_JointSpheres[i, p].GetComponent<Renderer>().material.color = skeletonColor;

            // Size of spheres
            float SphereRadius = 0.05f;
            m_JointSpheres[i, p].transform.localScale = new Vector3(SphereRadius, SphereRadius, SphereRadius);
        }

        // Next up create ellipsoids for bones
        int nBones = 18;
        m_Bones = new GameObject[nBones, m_totalNumPeople];
        for (int p = 0; p < m_totalNumPeople; ++p)
            for (int i = 0; i < nBones; ++i)
            {
                m_Bones[i, p] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                m_Bones[i,p].GetComponent<Renderer>().material.color = skeletonColor;
            }
    }
    
    public void drawSkeleton(int p,Vector3[] Joints,float LowestY)
    {

        // Make floor stick to bottom-most joint (at index 16 or 20)
        GameObject Plane = GameObject.Find("CheckerboardPlane");
        float PlaneFootBuffer = 0.02f;
        float MoveAmount = 0.0f;
        float difference_with_plane_and_feet = 0.0f;
        if (Plane != null)
        {
            Vector3 OrigPos = Plane.transform.position;

            MoveAmount = Plane.transform.position.y;

            difference_with_plane_and_feet = LowestY - MoveAmount - PlaneFootBuffer;

            for (int i = 0; i < m_JointSpheres.GetLength(0); ++i)
            {
                Vector3 orig = Joints[i];
                m_JointSpheres[i, p].transform.position = new Vector3(orig[0], orig[1] - difference_with_plane_and_feet, orig[2]);
            }

        }
        //1-spine1 spine 2
        drawEllipsoid(Joints[0], Joints[1], m_Bones[0,p], difference_with_plane_and_feet);
        //2-spine2 spine 3
        drawEllipsoid(Joints[1], Joints[2], m_Bones[1, p], difference_with_plane_and_feet);
        //3-spine 3 neck 1                   
        drawEllipsoid(Joints[2], Joints[3], m_Bones[2, p], difference_with_plane_and_feet);
        //4- neck 1 - head_ee                
        drawEllipsoid(Joints[3], Joints[4], m_Bones[3, p], difference_with_plane_and_feet);
  
        //2-spine_3_rx, 8-left_shoulder_ry
        drawEllipsoid(Joints[2], Joints[5], m_Bones[4, p], difference_with_plane_and_feet);
        //8-left_shoulder_ry, 9-left_elbow_rx
        drawEllipsoid(Joints[5], Joints[6], m_Bones[5, p], difference_with_plane_and_feet);
        //9-left_elbow_rx, 10-Hand_RX
        drawEllipsoid(Joints[6], Joints[7], m_Bones[6, p], difference_with_plane_and_feet);
        //10-Hand_RX, 11-hand ee
        drawEllipsoid(Joints[7], Joints[8], m_Bones[7, p], difference_with_plane_and_feet);

        //2-spine_3_rx, 8-right_shoulder_ry
        drawEllipsoid(Joints[2], Joints[13], m_Bones[8, p], difference_with_plane_and_feet);
        //8-left_shoulder_ry, 9-left_elbow_rx
        drawEllipsoid(Joints[13], Joints[14], m_Bones[9, p], difference_with_plane_and_feet);
        //9-left_elbow_rx, 10-Hand_RX
        drawEllipsoid(Joints[14], Joints[15], m_Bones[10, p], difference_with_plane_and_feet);
        //10-Hand_RX, 11-hand ee
        drawEllipsoid(Joints[15], Joints[16], m_Bones[11, p], difference_with_plane_and_feet);


        //4-spine_1_rx, 19-left_hip_ry
        drawEllipsoid(Joints[0], Joints[9], m_Bones[12, p], difference_with_plane_and_feet);
        //19-left_hip_ry, 20-left_knee_rx
        drawEllipsoid(Joints[9], Joints[10], m_Bones[13, p], difference_with_plane_and_feet);
        //20-left_knee_rx, 21-left_ankle_ry
        drawEllipsoid(Joints[10], Joints[11], m_Bones[14, p], difference_with_plane_and_feet);

        //4-spine_1_rx, 19-right_hip_ry
        drawEllipsoid(Joints[0], Joints[17], m_Bones[15, p], difference_with_plane_and_feet);
        //19-left_hip_ry, 20-right_knee_rx
        drawEllipsoid(Joints[17], Joints[18], m_Bones[16, p], difference_with_plane_and_feet);
        //20-left_knee_rx, 21-right_ankle_ry
        drawEllipsoid(Joints[18], Joints[19], m_Bones[17, p], difference_with_plane_and_feet);

 

        // Disable toe sphere
        m_JointSpheres[12, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[20, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[11, p].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[19, p].GetComponent<MeshRenderer>().enabled = false;
      

        // Draw mesh 21 20
        RFoot[p].transform.rotation = Quaternion.LookRotation((Joints[20] - Joints[19]).normalized);
        RFoot[p].transform.rotation = Quaternion.Euler(RFoot[p].transform.eulerAngles + new Vector3(140, 0, 0));
        RFoot[p].transform.position = Joints[19] - new Vector3(0, difference_with_plane_and_feet,0);

        // Rotate z-axis to align with bone vector 12 11
        LFoot[p].transform.rotation = Quaternion.LookRotation((Joints[12] - Joints[11]).normalized);
        LFoot[p].transform.rotation = Quaternion.Euler(LFoot[p].transform.eulerAngles + new Vector3(140, 0, 0));
        // Position at middle
        LFoot[p].transform.position = Joints[11]- new Vector3(0, difference_with_plane_and_feet, 0);

    }
    override public void Update(string Line)
    {

        //Debug.Log(Line);
        if (Input.GetKey(KeyCode.B)) //reset ball
        {


            m_Ball.transform.position = originalBallPos;
            m_Ball.transform.rotation = Quaternion.identity;
            m_Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }
        if (Input.GetKey(KeyCode.H)) //reset ball
            m_Ball.GetComponent<Renderer>().enabled = false;

        if (Input.GetKey(KeyCode.S)) //reset ball
            m_Ball.GetComponent<Renderer>().enabled = true;

        if (Line.Length == 0)
            return;

        // Parse line
        int ParseOffset = 0; // No offset for real-time system
        int num_bones = m_Bones.GetLength(0);
        string[] Tokens = Line.Split(',');
        Debug.Log("Detected " + (Tokens.Length - ParseOffset) / 3 + " joints.");

        int num_joints = 22;

        if ((Tokens.Length - ParseOffset) / 3 % num_joints != 0)
            return;
        
        Vector3[] Joints = new Vector3[(Tokens.Length - ParseOffset) / 3];

        int current_num_people = ((Tokens.Length - ParseOffset) / 3) / num_joints;
        Debug.Log("current_num_people " + current_num_people);
        for (int p = 0; p < current_num_people; ++p)
        {
            float LowestY = 999.0f;

            m_Feet[p].SetActive(true);

            for (int i = 0; i < m_JointSpheres.GetLength(0); ++i)
            {
                m_JointSpheres[i, p].SetActive(true);
                m_JointSpheres[i, p].GetComponent<Renderer>().material.color = skeletonColor * .5f;

                if (i < num_bones)
                {
                    m_Bones[i, p].SetActive(true);
                    m_Bones[i, p].GetComponent<Renderer>().material.color = skeletonColor;
                }

                Joints[i].x = float.Parse(Tokens[3 * p * num_joints + 3 * (i + 1) + 0 + ParseOffset]) * 0.001f; // Mirror for ease of viewing (-1), no mirror (1)
                Joints[i].y = float.Parse(Tokens[3 * p * num_joints + 3 * (i + 1) + 1 + ParseOffset]) * 0.001f;
                Joints[i].z = -float.Parse(Tokens[3 * p * num_joints + 3 * (i + 1) + 2 + ParseOffset]) * 0.001f; // Flip for Google VR

                if (Joints[i].y < LowestY)
                    LowestY = Joints[i].y;

                drawSkeleton(p, Joints, LowestY);
            }

        }
            for (int p = current_num_people; p < m_totalNumPeople; ++p)
            {
                m_Feet[p].SetActive(false);
                for (int i = 0; i < m_JointSpheres.GetLength(0); ++i)
                {
                    m_JointSpheres[i, p].SetActive(false);

                  if (i < num_bones)
                      m_Bones[i, p].SetActive(false);
                 }
            }
     }
}
