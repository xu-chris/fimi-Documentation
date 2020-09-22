using System;
using System.Collections.Generic;
using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SpeedRule : Rule
    {
        private class BoneDistance
        {
            internal BoneType type;
            internal Vector3 boneVector;
            internal float distance;

            public float CalculateAndStoreNewDistance(Vector3 newBoneVector)
            {
                distance = Vector3.Distance(boneVector, newBoneVector);
                boneVector = newBoneVector;
                return distance;
            }
        }
        
        public List<string> bones;
        public float lowerDistanceChangeThreshold;
        public float upperDistanceChangeThreshold;

        private readonly List<BoneDistance> lastDistancePerBone = new List<BoneDistance>();

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            var runningDistance = 0f;
            
            // Returns false if the bones aren't initialized at all.
            var initialized = false;
            
            foreach (var boneObject in boneObjects)
            {
                if (lastDistancePerBone.Exists(i => i.type == boneObject.boneType))
                {
                    var lastRecording = lastDistancePerBone.Find(i => i.type == boneObject.boneType);
                    initialized = true;
                    runningDistance += lastRecording.CalculateAndStoreNewDistance(boneObject.boneVector);
                }
                else
                {
                    lastDistancePerBone.Add(new BoneDistance
                    {
                        type = boneObject.boneType,
                        boneVector = boneObject.boneVector,
                        distance = 0f
                    });
                }
            }

            return initialized && (runningDistance < lowerDistanceChangeThreshold || runningDistance > upperDistanceChangeThreshold);
        }

        public override string ToString()
        {
            return "Rule: " + GetType().Name + ", lower distance change threshold: " + lowerDistanceChangeThreshold + ", upper distance change threshold:" + upperDistanceChangeThreshold + ", bones: " +
                   string.Join(", ", bones.ToArray());
        }
    }
}