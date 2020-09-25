using _Project.Scripts.DomainObjects;
using UnityEngine;

namespace _Project.Scripts.Core.InTraining
{
    public class InTrainingSkeletonOrchestrator : ISkeletonOrchestrator
    {
        private readonly int maxNumberOfPeople;
        private Exercise currentExercise;
        public ExerciseReport[] exerciseReports { get; }

        private InTrainingSkeleton[] skeletons;

        public InTrainingSkeletonOrchestrator(int maxNumberOfPeople, Exercise currentExercise)
        {
            this.maxNumberOfPeople = maxNumberOfPeople;
            this.currentExercise = currentExercise;
            exerciseReports = new ExerciseReport[maxNumberOfPeople];
            InitializeAllSkeletons();
        }

        public void Update(Person[] detectedPersons)
        {
            if (detectedPersons == null)
                return;

            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                // Init skeleton if not given.
                if (skeletons[p] == null)
                {
                    skeletons[p] = new InTrainingSkeleton(p);
                    Debug.LogError("Initialized a new skeleton which should be already there 🤔. p: " + p);
                }

                // Set and activate only skeletons that are detected.
                if (p >= 0 && detectedPersons.Length > p && p == detectedPersons[p].id)
                {
                    UpdateSkeleton(skeletons[p], detectedPersons[p]);
                    skeletons[p].CheckRules(currentExercise.rules);

                    exerciseReports[p] = skeletons[p].GetReport();
                }
                else
                {
                    skeletons[p].SetIsVisible(false);
                }
            }
        }

        public void InitializeAllSkeletons()
        {
            skeletons = new InTrainingSkeleton[maxNumberOfPeople];
            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                skeletons[p] = new InTrainingSkeleton(p);
                skeletons[p].SetIsVisible(false);
            }
        }

        private void UpdateSkeleton(InTrainingSkeleton skeleton, Person person)
        {
            skeleton.SetSkeleton(person.joints, person.lowestY);
            skeleton.SetIsVisible(true);
        }

        public void SetCurrentExercise(Exercise exercise)
        {
            currentExercise = exercise;
            foreach (var skeleton in skeletons) skeleton.SetUpExerciseReport(exercise);
        }
    }
}