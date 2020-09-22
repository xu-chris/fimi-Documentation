using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class SkeletonOrchestrator
    {
        private readonly int maxNumberOfPeople;

        private Exercise currentExercise;
        private InTrainingSkeleton[] skeletons;
        private List<int> validJointIdx;

        private Text reportTextField;

        public SkeletonOrchestrator(int maxNumberOfPeople, Text reportTextField)
        {
            this.maxNumberOfPeople = maxNumberOfPeople;
            this.reportTextField = reportTextField;
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
                    skeletons[p].SetSkeleton(detectedPersons[p].joints, detectedPersons[p].lowestY);
                    skeletons[p].SetIsVisible(true);
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

        private void InitializeAllSkeletons()
        {
            skeletons = new InTrainingSkeleton[maxNumberOfPeople];
            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                skeletons[p] = new InTrainingSkeleton(p);
                skeletons[p].SetIsVisible(false);
            }
        }
    }
}