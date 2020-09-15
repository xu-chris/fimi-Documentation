using System;
using UnityEngine;

public struct Joint
{
    public int ID;
    public String name;

    public GameObject gameObject;

    public Joint(int id, String name, Color color, float sphereRadius)
    {
        this.ID = id;
        this.name = name;

        this.gameObject = createGameObject(name, color, sphereRadius);
    }

    static GameObject createGameObject(String name, Color color, float sphereRadius)
    {
        GameObject newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newGameObject.name = name;
        newGameObject.GetComponent<Renderer>().material.color = color;
        newGameObject.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
        return newGameObject;
    }

    public void SetJointPosition(Vector3 jointPosition)
    {
        gameObject.transform.position = jointPosition;
    }

    public void SetIsVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}