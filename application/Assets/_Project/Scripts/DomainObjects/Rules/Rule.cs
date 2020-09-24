using System.Collections.Generic;
using _Project.Scripts.Core;

namespace _Project.Scripts.DomainObjects.Rules
{
    public abstract class Rule
    {
        public string notificationText;
        public bool colorize;
        public abstract bool IsInvalidated(List<Bone> boneObjects);
    }
}