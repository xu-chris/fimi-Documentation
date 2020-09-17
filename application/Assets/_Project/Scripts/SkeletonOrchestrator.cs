using System.Collections.Generic;
using _Project.Scripts.DomainObjects;
using _Project.Scripts.DomainObjects.Configurations;
using _Project.Scripts.DomainValues;
using UnityEngine;

namespace _Project.Scripts
{
    public class SkeletonOrchestrator
    {
        private readonly GameObject plane;
        private Skeleton[] skeletons;
        private List<int> validJointIdx;
        private readonly int maxNumberOfPeople;

        private Exercise currentExercise;

        public SkeletonOrchestrator(int maxNumberOfPeople, GameObject plane)
        {
            this.maxNumberOfPeople = maxNumberOfPeople;
            this.plane = plane;
            InitializeAllSkeletons();
        }

        public void Update(Person[] detectedPersons, float lowestY)
        {
            if (detectedPersons == null)
                return;

            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                // Init skeleton if not given.
                if (skeletons[p] == null)
                {
                    skeletons[p] = new Skeleton(p);
                    Debug.LogError("Initialized a new skeleton which should be already there 🤔. p: " + p);
                }

                // Set and activate only skeletons that are detected.
                if (p >= 0 && detectedPersons.Length > p && p == detectedPersons[p].id)
                {
                    skeletons[p].SetSkeleton(detectedPersons[p].joints, plane, lowestY);
                    skeletons[p].SetIsVisible(true);
                    skeletons[p].CheckRules(currentExercise.rules);
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
        }

        private void InitializeAllSkeletons()
        {
            skeletons = new Skeleton[maxNumberOfPeople];
            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                skeletons[p] = new Skeleton(p);
                skeletons[p].SetIsVisible(false);
            }
        }
    }
}
