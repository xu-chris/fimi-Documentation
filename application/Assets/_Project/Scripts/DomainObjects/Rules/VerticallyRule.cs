using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class VerticallyRule : Rule
    {
        public override RuleType type => RuleType.VERTICALLY_RULE;
        public override bool IsInvalidated(List<Bone> bones)
        {
            throw new System.NotImplementedException();
        }
    }
}