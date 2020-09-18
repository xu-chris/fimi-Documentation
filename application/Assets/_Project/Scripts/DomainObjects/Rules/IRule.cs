using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects.Rules
{
    public interface IRule
    {
        RuleType type { get; }
        bool IsInvalidated(List<Bone> boneObjects);
    }
}