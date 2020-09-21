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
                        isInvalided = rule.IsInvalidated(bonesConsideredForGivenRule);
                        GreenRedColoring(bonesConsideredForGivenRule, isInvalided);
                        if (isInvalided) exerciseReport.Count(angleRule);
                        break;
                    case RangeOfMotionRule rangeOfMotionRule:
                        bonesConsideredForGivenRule = rangeOfMotionRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = rule.IsInvalidated(bonesConsideredForGivenRule);
                        RedNeutralColoring(bonesConsideredForGivenRule, isInvalided);
                        if (isInvalided) exerciseReport.Count(rangeOfMotionRule);
                        break;
                    case SymmetryRule symmetryRule:
                        // TODO: Implement
                        break;
                    case LinearityRule linearityRule:
                        bonesConsideredForGivenRule = linearityRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = rule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(linearityRule);
                        if (isInvalided) Debug.Log("Bones " + linearityRule.bones + " are not parallel to each other.");
                        break;
                    case HorizontallyRule horizontallyRule:
                        // TODO: Implement
                        break;
                    case VerticallyRule verticallyRule:
                        bonesConsideredForGivenRule = verticallyRule.bones.ToBoneTypes().Select(GetBone).ToList();
                        isInvalided = rule.IsInvalidated(bonesConsideredForGivenRule);
                        if (isInvalided) exerciseReport.Count(verticallyRule);
                        if (isInvalided) Debug.Log("Bones " + verticallyRule.bones + " are not vertically aligned to each other.");
                        break;
                    case SpeedRule speedRule:
                        // TODO: Implement
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
    }
}