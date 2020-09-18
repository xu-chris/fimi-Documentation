using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts
{
    public class Joint
    {
        private readonly GameObject gameObject;
        private int jointIndex;
        private JointType jointType;

        public Joint(int jointIndex, JointType jointType, Color color, float sphereRadius, GameObject parentObject,
            bool createGameObject = true)
        {
            this.jointIndex = jointIndex;
            this.jointType = jointType;

            gameObject = createGameObject
                ? CreateGameObject(parentObject, jointType, color, sphereRadius)
                : new GameObject();
        }

        private static GameObject CreateGameObject(GameObject parentObject, JointType jointType, Color color,
            float sphereRadius)
        {
            var newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newGameObject.name = jointType.ToString();
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
            gameObject.transform.position = jointPosition;
        }
    }
}