using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public abstract class Rule : IRule
    {
        public abstract RuleType type { get; }
        public abstract bool IsInvalidated(List<Bone> boneObjects);
    }
}