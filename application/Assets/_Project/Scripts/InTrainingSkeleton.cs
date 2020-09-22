using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Rules;
using UnityEngine;

namespace _Project.Scripts
{
    public class InTrainingSkeleton : Skeleton
    {
        
        // Parameters
        private ExerciseReport exerciseReport;
        
        public InTrainingSkeleton(int id, bool withGameObjects = true) : base(id, withGameObjects)
        {
        }
        
        public void CheckRules(List<Rule> rules)
        {
            if (exerciseReport == null)
            {
                InitExerciseReport(rules);
            }
            
            foreach (var rule in rules)
            {
                List<Bone> bonesConsideredForGivenRule;
                bool isInvalided;
                switch (rule)
                {
                    case AngleRule angleRule:
                        bonesConsideredForGivenRule = angleRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = angleRule.IsInvalidated(bonesConsideredForGivenRule);
                        GreenRedColoring(bonesConsideredForGivenRule, isInvalided);
                        if (isInvalided) exerciseReport.Count(angleRule);
                        break;
                    case RangeOfMotionRule rangeOfMotionRule:
                        bonesConsideredForGivenRule = rangeOfMotionRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = rangeOfMotionRule.IsInvalidated(bonesConsideredForGivenRule);
                        RedNeutralColoring(bonesConsideredForGivenRule, isInvalided);
                        if (isInvalided) exerciseReport.Count(rangeOfMotionRule);
                        break;
                    case SymmetryRule symmetryRule:
                        var leftBones = symmetryRule.leftBones.ToBoneTypes().Select(GetBone).ToList();
                        var rightBones = symmetryRule.rightBones.ToBoneTypes().Select(GetBone).ToList();
                        var referenceBone = GetBone(symmetryRule.centerBone.ToBoneType());
                        isInvalided = symmetryRule.IsInvalidated(leftBones, rightBones, referenceBone);
                        if (isInvalided) exerciseReport.Count(symmetryRule);
                        break;
                    case LinearityRule linearityRule:
                        bonesConsideredForGivenRule = linearityRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = linearityRule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(linearityRule);
                        break;
                    case HorizontallyRule horizontallyRule:
                        bonesConsideredForGivenRule = horizontallyRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = horizontallyRule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(horizontallyRule);
                        break;
                    case VerticallyRule verticallyRule:
                        bonesConsideredForGivenRule = verticallyRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = verticallyRule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(verticallyRule);
                        break;
                    case SpeedRule speedRule:
                        bonesConsideredForGivenRule = speedRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = speedRule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(speedRule);
                        break;
                }
            }
        }
        
        private static void RedNeutralColoring(List<Bone> bones, bool colorRed)
        {
            var color = colorRed ? Color.red : skeletonColor;
            ColorizeAllBones(bones, color);
        }

        private static void GreenRedColoring(List<Bone> bones, bool colorToRed)
        {
            var color = colorToRed ? Color.red : Color.green;
            ColorizeAllBones(bones, color);
        }

        private static void ColorizeAllBones(List<Bone> bones, Color color)
        {
            foreach (var bone in bones) bone.Colorize(color);
        }

        private static void CreatePole(Vector2 from, Vector3 to)
        {
            var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        }

        public void SetUpExerciseReport(Exercise exercise)
        {
            InitExerciseReport(exercise.rules);
        }

        private void InitExerciseReport(List<Rule> rules)
        {
            exerciseReport = new ExerciseReport(rules);
        }

        public string GetReport()
        {
            return exerciseReport.ToString();
        }
    }
}