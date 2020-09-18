using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects
{
    public static class ListExtension
    {
        public static List<BoneType> ToBoneTypes(this List<string> bones)
        {
            return bones.Select(boneType => boneType.ToBoneType()).ToList();
        }
    }
}