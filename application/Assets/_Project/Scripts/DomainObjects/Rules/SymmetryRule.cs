using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SymmetryRule: Rule
    {
        public override RuleType type => RuleType.SYMMETRY_RULE;
        public List<string> leftBones;
        public List<string> rightBones;
        public string centerBone;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new System.NotImplementedException();
        }
    }
}