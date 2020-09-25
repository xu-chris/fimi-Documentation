using System.Collections.Generic;
using _Project.Scripts.Core;

namespace _Project.Scripts.DomainObjects.Rules
{
    public abstract class Rule
    {
        public bool colorize;
        public string notificationText;
        public abstract bool IsInvalidated(List<Bone> boneObjects);
    }
}