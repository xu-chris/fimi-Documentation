using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class SymmetryRuleTest : RuleTest
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
            // GIVEN
            var leftBone = CreateDummyBone(new Vector3(-1, 1, 0));
            var rightBone = CreateDummyBone(new Vector3(1, 1, 0));
            var referenceBone = CreateDummyBone(new Vector3(0, 1, 0));

            // WHEN
            var result = rule.IsInvalidated(new List<Bone>{leftBone}, new List<Bone>{rightBone}, referenceBone);

            // THEN
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ShouldReturnInvalidIfBothAreAsymmetrical()
        {
            // GIVEN
            var leftBone = CreateDummyBone(new Vector3(-1, 1, 0));
            var rightBone = CreateDummyBone(new Vector3(1, 1, tolerance + 1));
            var referenceBone = CreateDummyBone(new Vector3(0, 1, 0));

            // WHEN
            var result = rule.IsInvalidated(new List<Bone>{leftBone}, new List<Bone>{rightBone}, referenceBone);

            // THEN
            Assert.IsTrue(result);
        }
    }
}