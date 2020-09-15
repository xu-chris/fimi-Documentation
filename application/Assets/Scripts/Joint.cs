using UnityEngine;

public struct Joint
{
    public int ID;
    public string name;

    public GameObject gameObject;

    public Joint(int id, string name, Color color, float sphereRadius)
    {
        ID = id;
        this.name = name;

        gameObject = createGameObject(name, color, sphereRadius);
    }

    private static GameObject createGameObject(string name, Color color, float sphereRadius)
    {
        var newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
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