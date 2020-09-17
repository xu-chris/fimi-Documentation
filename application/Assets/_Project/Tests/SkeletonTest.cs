using _Project.Scripts;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainValues;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SkeletonTest
    {
        private Skeleton skeleton = new Skeleton(0, true);
        
        [Test]
        public void ShouldReturnTrueWhenInsideOfRuleBoundary()
        {
            // GIVEN
            var angle = 90;
            var tolerance = 10;
            var boneA = new Bone(BoneType.HEAD, 1, 2, Color.black, new GameObject(), false);
            var boneB = new Bone(BoneType.HEAD, 1, 2, Color.black, new GameObject(), false);

            var boneAStartVector = new Vector3(0, 0, 0);
            var boneAEndVector = new  Vector3(0, 0, 1);
            
            var boneBStartVector = new Vector3(0, 0, 0);
            var boneBEndVector = new Vector3(0, 1, 0);
            
            boneA.SetBoneSizeAndPosition(boneAStartVector, boneAEndVector, 0);
            boneB.SetBoneSizeAndPosition(boneBStartVector, boneBEndVector, 0);
            
            // WHEN
            var result = Skeleton.IsBonesInDegreeRange(angle, tolerance, tolerance, boneA, boneB);
            
            // THEN
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnFalseWhenOutsideOfRuleBoundary()
        {
            // GIVEN
            var angle = 10;
            var tolerance = 00;
            var boneA = new Bone(BoneType.HEAD, 1, 2, Color.black, new GameObject(), false);
            var boneB = new Bone(BoneType.HEAD, 1, 2, Color.black, new GameObject(), false);

            var boneAStartVector = new Vector3(0, 0, 0);
            var boneAEndVector = new  Vector3(0, 0, 1);
            
            var boneBStartVector = new Vector3(0, 0, 0);
            var boneBEndVector = new Vector3(0, 1, 0);
            
            boneA.SetBoneSizeAndPosition(boneAStartVector, boneAEndVector, 0);
            boneB.SetBoneSizeAndPosition(boneBStartVector, boneBEndVector, 0);
            
            // WHEN
            var result = Skeleton.IsBonesInDegreeRange(angle, tolerance, tolerance, boneA, boneB);
            
            // THEN
            Assert.IsFalse(result);
        }
    }
}