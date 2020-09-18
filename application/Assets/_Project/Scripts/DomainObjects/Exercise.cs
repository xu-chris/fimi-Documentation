using System;
using System.Collections.Generic;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects
{
    public class Exercise
    {
        public string type;
        public int repetitions;
        public string duration;
        public List<ExerciseAspect> rules;

        ExerciseType ExerciseType()
        {
            return type.ToExerciseType();
        }
    }
}