using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class LinearityRule : Rule
    {
        public override RuleType type => RuleType.LINEARITY_RULE;
        public override bool IsInvalidated(List<Bone> bones)
        {
            throw new System.NotImplementedException();
        }
    }
}