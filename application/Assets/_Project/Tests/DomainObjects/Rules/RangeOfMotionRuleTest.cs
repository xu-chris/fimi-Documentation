using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class RangeOfMotionRuleTest
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
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.right
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfLowerThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (lowerThreshold - 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfLowerThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (lowerThreshold + 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfUpperThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (upperThreshold + 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfUpperThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down, (upperThreshold - 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }
    }
}