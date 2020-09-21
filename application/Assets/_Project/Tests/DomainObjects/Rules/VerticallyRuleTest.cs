using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class VerticallyRuleTest
    {
        private VerticallyRule verticallyRule;

        [SetUp]
        public void SetUp()
        {
            verticallyRule = new VerticallyRule
            {
                tolerance = 1f
            };
        }

        [Test]
        public void ShouldCountNoDifferenceInYAsValid()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.back
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidWhenBoneIsToLeft()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.left
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneOfTwoBonesIsToLeft()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.left
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidForUpVectors()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            Assert.IsFalse(result);
        }
    }
}