using System;
using UnityEngine;

public struct Bone
{
    public String name;
    public int jointA;
    public int jointB;

    public GameObject gameObject;

    public Bone(String name, int jointA, int jointB)
    {
        this.name = name;
        this.jointA = jointA;
        this.jointB = jointB;
        this.gameObject = CreateGameObject(name, Color.black);
    }
    
    public Bone(String name, int jointA, int jointB, Color color)
    {
        this.name = name;
        this.jointA = jointA;
        this.jointB = jointB;
        this.gameObject = CreateGameObject(name, color);
    }

    private static GameObject CreateGameObject(String name, Color color)
    {
        GameObject newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newGameObject.name = name;
        newGameObject.GetComponent<Renderer>().material.color = color;

        return newGameObject;
    }

    public void Colorize(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void SetBoneSizeAndPosition(Vector3 start, Vector3 end, float shift)
    {
        // Go to unit sphere
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        var boneVec = end - start;

        // Set z-axis of sphere to align with bone
        var zScale = boneVec.magnitude * 0.95f;
        var xyScale = zScale * 0.28f;
        gameObject.transform.localScale = new Vector3(xyScale, xyScale, zScale);

        // Reducing noise 
        if (!(boneVec.magnitude > 0.00001)) return;

        // Rotate z-axis to align with bone vector
        gameObject.transform.rotation = Quaternion.LookRotation(boneVec.normalized);
        // Position at middle
        gameObject.transform.position = (start + end) / 2.0f - new Vector3(0, shift, 0);
    }
    
    public void SetIsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}