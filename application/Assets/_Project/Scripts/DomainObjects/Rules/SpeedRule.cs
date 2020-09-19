using System;
using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class SpeedRule : Rule
    {
        public override RuleType type => RuleType.SPEED_RULE;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new NotImplementedException();
        }
    }
}