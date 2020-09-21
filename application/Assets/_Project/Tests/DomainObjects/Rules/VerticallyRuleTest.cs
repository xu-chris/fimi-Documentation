using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class VerticallyRuleTest : RuleTest
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
            // GIVEN
            var bone = CreateDummyBone(Vector3.back);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidWhenBoneIsToLeft()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.left);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneOfTwoBonesIsToLeft()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.left);
            var bone2 = CreateDummyBone(Vector3.up);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidForUpVectors()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            // THEN
            Assert.IsFalse(result);
        }
    }
}