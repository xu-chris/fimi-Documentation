using System.Collections.Generic;

namespace _Project.Scripts.DomainObjects.Rules
{
    public interface IRule
    {
        bool IsInvalidated(List<Bone> boneObjects);
    }
}