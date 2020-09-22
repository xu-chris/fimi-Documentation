using _Project.Scripts.DomainObjects;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Core.InTraining
{
    public class InTrainingSkeletonOrchestrator : SkeletonOrchestrator
    {
        private Text reportTextField;
        private Exercise currentExercise;
        
        public InTrainingSkeletonOrchestrator(int maxNumberOfPeople, Text reportTextField) : base(maxNumberOfPeople)
        {
            this.reportTextField = reportTextField;
        }
        
        public override void Update(Person[] detectedPersons)
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
                    reportTextField.text = skeletons[p].GetReport();
                }
                else
                {
                    skeletons[p].SetIsVisible(false);
                }
            }
        }
        
        public void SetCurrentExercise(Exercise exercise)
        {
            currentExercise = exercise;
            foreach (var skeleton in skeletons)
            {
                skeleton.SetUpExerciseReport(exercise);
            }
        }
    }
}