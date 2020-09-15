using UnityEngine;

public struct Bone
{
    private BoneType _boneType;
    public readonly int JointA;
    public readonly int JointB;

    private readonly GameObject _gameObject;

    public Bone(BoneType boneType, int jointA, int jointB)
    {
        this._boneType = boneType;
        this.JointA = jointA;
        this.JointB = jointB;
        _gameObject = CreateGameObject(boneType, Color.black);
    }

    public Bone(BoneType boneType, int jointA, int jointB, Color color)
    {
        this._boneType = boneType;
        this.JointA = jointA;
        this.JointB = jointB;
        _gameObject = CreateGameObject(boneType, color);
    }

    private static GameObject CreateGameObject(BoneType name, Color color)
    {
        var newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newGameObject.name = name.ToString();
        newGameObject.GetComponent<Renderer>().material.color = color;

        return newGameObject;
    }

    public void Colorize(Color color)
    {
        _gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void SetBoneSizeAndPosition(Vector3 start, Vector3 end, float shift)
    {
        // Go to unit sphere
        _gameObject.transform.position = Vector3.zero;
        _gameObject.transform.rotation = Quaternion.identity;
        _gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        var boneVec = end - start;

        // Set z-axis of sphere to align with bone
        var zScale = boneVec.magnitude * 0.95f;
        var xyScale = zScale * 0.28f;
        _gameObject.transform.localScale = new Vector3(xyScale, xyScale, zScale);

        // Reducing noise 
        if (!(boneVec.magnitude > 0.00001)) return;

        // Rotate z-axis to align with bone vector
        _gameObject.transform.rotation = Quaternion.LookRotation(boneVec.normalized);
        // Position at middle
        _gameObject.transform.position = (start + end) / 2.0f - new Vector3(0, shift, 0);
    }

    public void SetIsVisible(bool visible)
    {
        _gameObject.SetActive(visible);
    }
}