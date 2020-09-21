using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.DomainObjects.Rules
{
    /// <summary>
    ///     Vertically is defined as an angle difference of 0 between a given set of bones and a directional vector.
    ///     Given the circumstances, we define Vector2(0, 1) as our reference vector which means up straight in the Y
    ///     direction. We ignore the Z axis for our calculation completely.
    /// </summary>
    public class VerticallyRule : Rule
    {
        private readonly string axis = "y"; // Y or Z axis
        public List<string> bones;
        public float tolerance;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            var referenceVector = new Vector2(0, 1);
            var angleDifference = 0f;
            foreach (var bone in boneObjects)
            {
                var yAxisForComparison = axis == "y" ? bone.boneVector.y : bone.boneVector.z;
                var comparisonVector = new Vector2(bone.boneVector.x, yAxisForComparison);
                angleDifference += Vector2.Angle(referenceVector, comparisonVector);
            }

            return angleDifference > tolerance || angleDifference < -tolerance;
        }

        public override string ToString()
        {
            return "Rule: " + GetType().Name + ", tolerance: " + tolerance + ", bones: " +
                   string.Join(", ", bones.ToArray());
        }
    }
}