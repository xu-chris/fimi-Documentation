using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class HorizontallyRule : Rule
    {
        public List<string> bones;
        public float tolerance;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            var runningAngle = 0f;
            var plane = new Plane(Vector3.up, Vector3.zero);
            foreach (var bone in boneObjects)
            {
                var referenceVector = plane.normal;
                runningAngle += 90 - Vector3.Angle(bone.boneVector, referenceVector);
            }

            return runningAngle < -tolerance || runningAngle > tolerance;
        }

        public override string ToString()
        {
            return "Rule: " + GetType().Name + ", tolerance: " + tolerance + ", bones: " +
                   string.Join(", ", bones.ToArray());
        }
    }
}