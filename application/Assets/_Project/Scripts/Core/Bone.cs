using _Project.Scripts.DomainValues;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Core
{
    public class Bone
    {
        internal readonly BoneType boneType;

        private readonly GameObject gameObject;
        public readonly int jointIndexA;
        public readonly int jointIndexB;
        public Vector3 boneVector;

        public Bone(BoneType boneType, int jointIndexA, int jointIndexB, Color color, GameObject parentObject,
            bool createGameObject = true)
        {
            this.boneType = boneType;
            this.jointIndexA = jointIndexA;
            this.jointIndexB = jointIndexB;
            gameObject = createGameObject ? InitGameObject(parentObject, boneType, color) : new GameObject();

            boneVector = Vector3.zero;
        }

        private static GameObject InitGameObject(GameObject parentObject, BoneType name, Color color)
        {
            var newGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newGameObject.name = name.ToString();
            newGameObject.transform.parent = parentObject.transform;
            newGameObject.GetComponent<Renderer>().material.color = color;

            return newGameObject;
        }

        public void Colorize(Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }

        /**
         * Translates, rotates and scales the bone based on the given start and end points plus shift.
         */
        public void SetBoneSizeAndPosition(Vector3 start, Vector3 end, float lowestY)
        {
            // Go to unit sphere
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            boneVector = end - start;

            // Set z-axis of sphere to align with bone
            var zScale = boneVector.magnitude * 0.95f;
            var xyScale = zScale * 0.28f;
            gameObject.transform.localScale = new Vector3(xyScale, xyScale, zScale);

            // Reducing noise 
            if (!(boneVector.magnitude > 0.00001)) return;

            // Rotate z-axis to align with bone vector
            gameObject.transform.rotation = Quaternion.LookRotation(boneVector.normalized);
            // Position at middle
            gameObject.transform.position = (start + end) / 2.0f - new Vector3(0, lowestY, 0);

            Assert.AreEqual(boneVector, end - start);
        }
    }
}