using System.Collections.Generic;
using _Project.Scripts.DomainValues;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable ClassNeverInstantiated.Global - Class is instantiated by the YAML configuration reader

namespace _Project.Scripts.DomainObjects.Rules
{
    public class AngleRule : Rule
    {
        public float expectedAngle;
        public float lowerTolerance;
        public float upperTolerance;
        public override RuleType type => RuleType.ANGLE;

        public override bool IsInvalidated(List<Bone> bones)
        {
            Assert.IsTrue(bones.Count == 2, "You need to specify exactly two bones to check with this rule.");
            
            var calculatedAngle = Vector3.Angle(bones[0].boneVector, bones[1].boneVector);
            return (calculatedAngle < expectedAngle - lowerTolerance) || (calculatedAngle > upperTolerance + expectedAngle);
        }
    }
}