using System;
using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SymmetryRule : Rule
    {
        public string centerBone;
        public List<string> leftBones;
        public List<string> rightBones;
        public override RuleType type => RuleType.SYMMETRY_RULE;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new NotImplementedException();
        }
    }
}