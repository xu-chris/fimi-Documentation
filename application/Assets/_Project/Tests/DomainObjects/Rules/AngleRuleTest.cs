using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class AngleRuleTest
    {
        private const int expectedAngle = 90;
        private const int lowerTolerance = 20;
        private const int upperTolerance = 5;
        private readonly List<string> bones = new List<string> {"RightHand", "LeftHand"};
        private AngleRule rule;


        [SetUp]
        public void SetUp()
        {
            rule = new AngleRule
            {
                expectedAngle = expectedAngle,
                lowerTolerance = lowerTolerance,
                upperTolerance = upperTolerance,
                bones = bones
            };
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
                boneVector = Vector3.up
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfExactExpectedAngle()
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
        public void ShouldReturnInvalidIfOutsideOfLowerTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down,
                    (expectedAngle - lowerTolerance - 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfLowerTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down,
                    (expectedAngle - lowerTolerance + 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOutsideOfUpperTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down,
                    (expectedAngle + upperTolerance + 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfInsideOfUpperTolerance()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.up, Vector3.down,
                    (expectedAngle + upperTolerance - 1) * Mathf.Deg2Rad, 1)
            };

            var result = rule.IsInvalidated(new List<Bone> {bone1, bone2});

            Assert.IsFalse(result);
        }
    }
}