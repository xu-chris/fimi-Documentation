using System;
using System.Collections.Generic;
using _Project.Scripts.Core;
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
                var flippedVector = RotateAround(leftBoneObject.boneVector, referenceBone.boneVector, referenceBone.boneVector, 180);
                reflectedLeftBoneVectors.Add(flippedVector);

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

        public override string ToString()
        {
            return "Rule: " + GetType().Name + ", tolerance: " + tolerance + ", left bones: " +
                   string.Join(", ", leftBones.ToArray()) + ", right bones: " +
                   string.Join(", ", leftBones.ToArray()) + ", center / reference bone: " + centerBone;
        }
        
        private Vector3 RotateAround(Vector3 fromPoint, Vector3 aroundPoint, Vector3 aroundAxis, float angle)
        {
            Vector3 vector3 = Quaternion.AngleAxis(angle, aroundAxis) * (fromPoint - aroundPoint);
            return aroundPoint + vector3;
        }
    }
}