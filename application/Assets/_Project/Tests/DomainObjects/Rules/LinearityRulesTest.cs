using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class LinearityRulesTest : RuleTest
    {
        private const float tolerance = 5f;
        private readonly List<string> bones = new List<string> {"LeftForearm", "LeftElbow"};
        private LinearityRule rule;


        [SetUp]
        public void SetUp()
        {
            rule = new LinearityRule
            {
                bones = bones,
                tolerance = tolerance
            };
        }

        [Test]
        public void ShouldReturnInvalidIfPerpendicular()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateDummyBone(Vector3.left);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfExactLinear()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateDummyBone(Vector3.down);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfTolerance()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(tolerance + 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfTolerance()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(tolerance - 1);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidWhenChainOfBonesIsSlightlyTurnedButInsideOfTolerance()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.up);
            var bone2 = CreateRotatedDummyBone(tolerance - 1);
            var bone3 = CreateDummyBone(Vector3.down);

            // WHEN
            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2, bone3});

            // THEN
            Assert.IsFalse(result);
        }
    }
}