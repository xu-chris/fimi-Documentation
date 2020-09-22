using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class RangeOfMotionRuleTest : RuleTest
    {
        private const float lowerThreshold = 70f;
        private const float upperThreshold = 100f;
        private readonly List<string> bones = new List<string> {"RightHand", "LeftHand"};
        private RangeOfMotionRule rule;


        [SetUp]
        public void SetUp()
        {
            rule = new RangeOfMotionRule
            {
                lowerThreshold = lowerThreshold,
                upperThreshold = upperThreshold,
                bones = bones
            };
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateDummyBone(Vector3.up);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateDummyBone(Vector3.right);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfLowerThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(lowerThreshold - 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfLowerThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(lowerThreshold + 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfUpperThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(upperThreshold + 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfUpperThreshold()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(upperThreshold - 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsFalse(result);
        }
    }
}