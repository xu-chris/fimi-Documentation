using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class HorizontallyRuleTest : RuleTest
    {
        private readonly float tolerance = 1f;
        private HorizontallyRule verticallyRule;

        [SetUp]
        public void SetUp()
        {
            verticallyRule = new HorizontallyRule
            {
                tolerance = tolerance
            };
        }

        [Test]
        public void ShouldReturnInvalidForPerpendicularVector()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.up);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidForLeftVector()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.left);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForRightVector()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.right);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForBackVector()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.back);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForForwardVector()
        {
            // GIVEN
            var bone = CreateDummyBone(Vector3.forward);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneBoneIsOverThreshold()
        {
            // GIVEN
            var bone1 = CreateRotatedDummyBone(tolerance + 1);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfOneBoneIsInThreshold()
        {
            // GIVEN
            var bone1 = CreateRotatedDummyBone(tolerance - 1);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidIfOneBoneIsInNegativeThreshold()
        {
            // GIVEN
            var bone1 = CreateRotatedDummyBone(-tolerance + 1);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            // THEN
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneBoneIsOutOfNegativeThreshold()
        {
            // GIVEN
            var bone1 = CreateRotatedDummyBone(-tolerance - 1);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnInvalidIfAChainOfBonesIsOutOfNegativeThreshold()
        {
            // GIVEN
            var bone1 = CreateRotatedDummyBone(-tolerance - 1);
            var bone2 = CreateDummyBone(Vector3.left);
            var bone3 = CreateDummyBone(Vector3.right);

            // WHEN
            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1, bone2, bone3});

            // THEN
            Assert.IsTrue(result);
        }

        private new static Bone CreateRotatedDummyBone(float degree)
        {
            return CreateDummyBone(Vector3.RotateTowards(Vector3.left, Vector3.up, degree * Mathf.Deg2Rad, 1));
        }
    }
}