using System.Collections.Generic;
using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class LinearityRule : Rule
    {
        public List<string> bones;
        public float tolerance;
        public override RuleType type => RuleType.LINEARITY_RULE;
        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            var runningDotProduct = 0f;
            for (var i = 0; i < boneObjects.Count - 1; i++)
            {
                runningDotProduct += Vector3.Dot(boneObjects[i].boneVector, boneObjects[i+1].boneVector);   
            }

            return (runningDotProduct > tolerance || runningDotProduct < -tolerance);
        }
    }
}