using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class LinearityRule : Rule
    {
        public List<string> bones;
        public float tolerance;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            var runningAngle = 0f;
            for (var i = 0; i < boneObjects.Count - 1; i++)
                runningAngle += Vector3.Angle(boneObjects[i].boneVector, boneObjects[i + 1].boneVector);

            // Allow both 0 and 180 degree
            runningAngle %= 180;

            return runningAngle > tolerance || runningAngle < -tolerance;
        }

        public override string ToString()
        {
            return "Rule: " + GetType().Name + ", tolerance: " + tolerance + ", bones: " +
                   string.Join(", ", bones.ToArray());
        }
    }
}