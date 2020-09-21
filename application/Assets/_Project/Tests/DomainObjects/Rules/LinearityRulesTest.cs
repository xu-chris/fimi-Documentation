using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class LinearityRulesTest
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
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.left
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfExactLinear()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.down
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (tolerance + 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (tolerance - 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidWhenChainOfBonesIsSlightlyTurnedButInsideOfTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (tolerance - 1) * Mathf.Deg2Rad, 1)
            };

            var bone3 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.down
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2, bone3});

            Assert.IsFalse(result);
        }
    }
}