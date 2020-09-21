using System.Collections.Generic;

namespace _Project.Scripts.DomainObjects.Rules
{
    public abstract class Rule : IRule
    {
        public abstract bool IsInvalidated(List<Bone> boneObjects);
    }
}