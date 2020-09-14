using UnityEngine;

public abstract class RunLive
{
    protected GameObject[] LFoot;
    protected GameObject[,] m_Bones;
    protected GameObject[] m_Feet;
    public GameObject[,] m_JointSpheres;
    protected int m_totalNumPeople = 10;
    protected GameObject[] RFoot;


    public abstract void Start();
    public abstract void Update(string line);

    protected static void DrawEllipsoid(Vector3 start, Vector3 end, GameObject bone, float shift)
    {
        // Go to unit sphere
        bone.transform.position = Vector3.zero;
        bone.transform.rotation = Quaternion.identity;
        bone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        var boneVec = end - start;

        // Set z-axis of sphere to align with bone
        var zScale = boneVec.magnitude * 0.95f;
        var xyScale = zScale * 0.28f;
        bone.transform.localScale = new Vector3(xyScale, xyScale, zScale);

        // Reducing noise 
        if (!(boneVec.magnitude > 0.00001)) return;

        // Rotate z-axis to align with bone vector
        bone.transform.rotation = Quaternion.LookRotation(boneVec.normalized);
        // Position at middle
        bone.transform.position = (start + end) / 2.0f - new Vector3(0, shift, 0);
    }
}