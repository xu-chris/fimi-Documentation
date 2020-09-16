using _Project.Scripts.DomainValues;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts
{
    public struct Bone
    {
        internal readonly BoneType BoneType;
        public readonly int JointIndexA;
        public readonly int JointIndexB;
        public Vector3 BoneVector;

        private readonly GameObject _gameObject;

        public Bone(BoneType boneType, int jointIndexA, int jointIndexB, Color color, GameObject parentObject, bool createGameObject = true)
        {
            this.BoneType = boneType;
            this.JointIndexA = jointIndexA;
            this.JointIndexB = jointIndexB;
            _gameObject = createGameObject ? InitGameObject(parentObject, boneType, color) : new GameObject();
            
            BoneVector = Vector3.zero;
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
            _gameObject.GetComponent<Renderer>().material.color = color;
        }

        /**
         * Translates, rotates and scales the bone based on the given start and end points plus shift.
         */
        public Bone SetBoneSizeAndPosition(Vector3 start, Vector3 end, float shift)
        {
            // Go to unit sphere
            _gameObject.transform.position = Vector3.zero;
            _gameObject.transform.rotation = Quaternion.identity;
            _gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            BoneVector = end - start;

            // Set z-axis of sphere to align with bone
            var zScale = BoneVector.magnitude * 0.95f;
            var xyScale = zScale * 0.28f;
            _gameObject.transform.localScale = new Vector3(xyScale, xyScale, zScale);

            // Reducing noise 
            if (!(BoneVector.magnitude > 0.00001)) return this;

            // Rotate z-axis to align with bone vector
            _gameObject.transform.rotation = Quaternion.LookRotation(BoneVector.normalized);
            // Position at middle
            _gameObject.transform.position = (start + end) / 2.0f - new Vector3(0, shift, 0);
            
            Assert.AreEqual(BoneVector, end - start);

            return this;
        }
    }
}
