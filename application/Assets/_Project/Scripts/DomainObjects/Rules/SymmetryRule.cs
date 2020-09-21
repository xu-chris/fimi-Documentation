using System;
using System.Collections.Generic;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SymmetryRule : Rule
    {
        public string centerBone;
        public List<string> leftBones;
        public List<string> rightBones;
        public float tolerance;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new NotImplementedException();
        }
    }
}