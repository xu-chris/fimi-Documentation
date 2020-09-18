using System;
using System.Collections.Generic;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects
{
    public class Exercise
    {
        public string type;
        public int repetitions;
        public string duration;
        public List<Rule> rules;

        ExerciseType ExerciseType()
        {
            return type.ToExerciseType();
        }
    }
}