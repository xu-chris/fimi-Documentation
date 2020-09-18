using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class HorizontallyRule : Rule
    {
        public List<string> bones;
        public override RuleType type => RuleType.HORIZONTALLY_RULE;
        public override bool IsInvalidated(List<Bone> bones)
        {
            throw new System.NotImplementedException();
        }
    }
}