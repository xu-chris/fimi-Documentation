using System.Collections.Generic;
using _Project.Scripts.DomainObjects.Rules;
using _Project.Scripts.DomainValues;

namespace _Project.Scripts.DomainObjects
{
    public class Exercise
    {
        public string duration;
        public int repetitions;
        public List<Rule> rules;
        public string type;

        private ExerciseType ExerciseType()
        {
            return type.ToExerciseType();
        }
    }
}