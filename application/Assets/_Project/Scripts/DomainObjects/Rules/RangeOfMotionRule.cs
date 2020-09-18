using System.Collections.Generic;
using _Project.Scripts.DomainValues;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable ClassNeverInstantiated.Global - Class is instantiated by the YAML configuration reader

namespace _Project.Scripts.DomainObjects.Rules
{
    public class RangeOfMotionRule : Rule
    {
        public List<string> bones;
        public float lowerThreshold;
        public float upperThreshold;
        public override RuleType type => RuleType.RANGE_OF_MOTION;

        public override bool IsInvalidated(List<Bone> bones)
        {
            Assert.IsTrue(bones.Count == 2, "You need to specify exactly two bones to check with this rule.");
            
            var calculatedAngle = Vector3.Angle(bones[0].boneVector, bones[1].boneVector);
            return (calculatedAngle > upperThreshold) || (calculatedAngle < lowerThreshold);
        }
    }
}