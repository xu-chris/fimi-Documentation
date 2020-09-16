using UnityEngine;

namespace _Project.Scripts
{
    public struct Joint
    {
        private int _id;
        private string _name;

        private GameObject _gameObject;

        public Joint(int id, string name, Color color, float sphereRadius, GameObject parentObject)
        {
            _id = id;
            this._name = name;

            _gameObject = CreateGameObject(parentObject, name, color, sphereRadius);
        }

        private static GameObject CreateGameObject(GameObject parentObject, string name, Color color,
            float sphereRadius)
        {
            var newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newGameObject.name = name;
            newGameObject.transform.parent = parentObject.transform;
            newGameObject.GetComponent<Renderer>().material.color = color;
            newGameObject.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
            return newGameObject;
        }

        /**
     * Moves the joint to the dedicated position.
     * Because moving might be because it was detected, it will set to activated as well.
     */
        public void SetJointPosition(Vector3 jointPosition)
        {
            _gameObject.transform.position = jointPosition;
            _gameObject.SetActive(true);
        }
    }
}