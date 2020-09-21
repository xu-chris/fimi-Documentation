using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DomainObjects.Rules
{
    public class HorizontallyRuleTest
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
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.up
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidForLeftVector()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.left
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForRightVector()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.right
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForBackVector()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.back
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidForForwardVector()
        {
            var bone = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.forward
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneBoneIsOverThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.left, Vector3.up, tolerance + 1, 1)
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnValidIfOneBoneIsInThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.left, Vector3.up, tolerance - 1, 1)
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnValidIfOneBoneIsInNegativeThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.left, Vector3.up, -tolerance + 1, 1)
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnInvalidIfOneBoneIsOutOfNegativeThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.left, Vector3.up, -tolerance - 1, 1)
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1});

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnInvalidIfAChainOfBonesIsOutOfNegativeThreshold()
        {
            var bone1 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.RotateTowards(Vector3.left, Vector3.up, -tolerance - 1, 1)
            };

            var bone2 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.left
            };

            var bone3 = new Bone(BoneType.HEAD, 0, 1, Color.black, new GameObject(), false)
            {
                boneVector = Vector3.right
            };

            var result = verticallyRule.IsInvalidated(new List<Bone> {bone1, bone2, bone3});

            Assert.IsTrue(result);
        }
    }
}