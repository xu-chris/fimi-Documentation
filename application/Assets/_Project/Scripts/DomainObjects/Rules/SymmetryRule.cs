using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SymmetryRule : Rule
    {
        public string centerBone;
        public List<string> leftBones;
        public List<string> rightBones;
        public float tolerance;

        public bool IsInvalidated(List<Bone> leftBoneObjects, List<Bone> rightBoneObjects, Bone referenceBone)
        {
            var reflectedLeftBoneVectors = new List<Vector3>();
                
            // Reflect bone vectors of one list
            foreach (var leftBoneObject in leftBoneObjects)
            {
                var gameObject = new GameObject();
                gameObject.transform.position = leftBoneObject.boneVector;
                gameObject.transform.RotateAround(referenceBone.boneVector, referenceBone.boneVector, 180);
                reflectedLeftBoneVectors.Add(gameObject.transform.position);
                Object.Destroy(gameObject);
            }

            var runningDistance = 0f;
            
            foreach (var leftBoneVector in reflectedLeftBoneVectors)
            foreach (var rightBoneObject in rightBoneObjects)
            {
                runningDistance += Vector3.Distance(leftBoneVector, rightBoneObject.boneVector);
            }

            return runningDistance > tolerance;
        }

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new NotImplementedException();
        }
    }
}