using System;
using System.Collections.Generic;

namespace _Project.Scripts.DomainObjects.Rules
{
    public class HorizontallyRule : Rule
    {
        public List<string> bones;

        public override bool IsInvalidated(List<Bone> boneObjects)
        {
            throw new NotImplementedException();
        }
    }
}