using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class SymmetryRuleTest
    {
        private const float tolerance = 5f;
        private readonly List<string> leftSideBones = new List<string> {"LeftForearm", "LeftElbow"};
        private readonly List<string> rightSideBones = new List<string> {"RightForearm", "RightElbow"};
        private SymmetryRule rule;


        [SetUp]
        public void SetUp()
        {
            rule = new SymmetryRule
            {
                leftBones = leftSideBones,
                rightBones = rightSideBones,
                tolerance = tolerance
            };
        }

        [Test]
        public void ShouldReturnValidIfBothAreSymmetrical()
        {
            var leftBone = new Bone(BoneType.LEFT_HAND, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(-1, 1, 0)
            };
            
            var rightBone = new Bone(BoneType.RIGHT_HAND, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(1, 1, 0)
            };
            
            var referenceBone = new Bone(BoneType.LOWER_BODY, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(0, 1, 0)
            };

            var result = rule.IsInvalidated(new List<Bone>{leftBone}, new List<Bone>{rightBone}, referenceBone);

            Assert.IsFalse(result);
        }
        
        [Test]
        public void ShouldReturnInvalidIfBothAreAsymmetrical()
        {
            var leftBone = new Bone(BoneType.LEFT_HAND, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(-1, 1, 0)
            };
            
            var rightBone = new Bone(BoneType.RIGHT_HAND, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(1, 1, tolerance + 1)
            };
            
            var referenceBone = new Bone(BoneType.LOWER_BODY, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = new Vector3(0, 1, 0)
            };

            var result = rule.IsInvalidated(new List<Bone>{leftBone}, new List<Bone>{rightBone}, referenceBone);

            Assert.IsTrue(result);
        }
    }
}