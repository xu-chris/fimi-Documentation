using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class SpeedRuleTest : RuleTest
    {
        private SpeedRule rule;

        private float lowerDistanceChangeThreshold = 0.1f;
        private float upperDistanceChangeThreshold = 10f;

        [SetUp]
        public void SetUp()
        {
            rule = new SpeedRule()
            {
                lowerDistanceChangeThreshold = lowerDistanceChangeThreshold,
                upperDistanceChangeThreshold = upperDistanceChangeThreshold
            };
        }

        [Test]
        public void ShouldReturnValidIfInGoodSpeed()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.back);
            var result0 = rule.IsInvalidated(new List<Bone> {bone1});
            bone1.boneVector.z += lowerDistanceChangeThreshold;

            // WHEN
            var result1 = rule.IsInvalidated(new List<Bone> {bone1});
            
            // THEN
            Assert.IsFalse(result1);
        }

        [Test]
        public void ShouldReturnInvalidIfInGodSpeed()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.back);
            var result0 = rule.IsInvalidated(new List<Bone> {bone1});
            bone1.boneVector.x = upperDistanceChangeThreshold + 0.1f;

            // WHEN
            var result1 = rule.IsInvalidated(new List<Bone> {bone1});
            
            // THEN
            Assert.IsTrue(result1);
        }

        [Test]
        public void ShouldReturnInvalidIfNotMovingAtAll()
        {
            // GIVEN
            var bone1 = CreateDummyBone(Vector3.left);
            var result0 = rule.IsInvalidated(new List<Bone> {bone1});
            
            // WHEN
            var result1 = rule.IsInvalidated(new List<Bone> {bone1});
            
            // THEN
            Assert.IsFalse(result0);
            Assert.IsTrue(result1);   
        }
    }
}