using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable ClassNeverInstantiated.Global - Class is instantiated by the YAML configuration reader

namespace _Project.Scripts.DomainObjects.Rules
{
    public class AngleRule : Rule
    {
        public List<string> bones;
        public float expectedAngle;
        public float lowerTolerance;
        public float upperTolerance;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            Assert.IsTrue(boneObjects.Count == 2, "You need to specify exactly two bones to check with this rule.");

            var calculatedAngle = Vector3.Angle(boneObjects[0].boneVector, boneObjects[1].boneVector);
            return calculatedAngle < expectedAngle - lowerTolerance || calculatedAngle > upperTolerance + expectedAngle;
        }
    }
}