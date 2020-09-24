using System.Collections.Generic;
using _Project.Scripts.Core.InTraining;
using _Project.Scripts.DomainObjects;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class SkeletonOrchestrator
    {
        protected readonly int maxNumberOfPeople;

        private Exercise currentExercise;
        protected InTrainingSkeleton[] skeletons;
        private List<int> validJointIdx;

        

        public SkeletonOrchestrator(int maxNumberOfPeople)
        {
            this.maxNumberOfPeople = maxNumberOfPeople;
            InitializeAllSkeletons();
        }

        public virtual void Update(Person[] detectedPersons)
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
                }
                else
                {
                    skeletons[p].SetIsVisible(false);
                }
            }
        }
        
        protected virtual void UpdateSkeleton(Skeleton skeleton, Person person)
        {
            skeleton.SetSkeleton(person.joints, person.lowestY);
            skeleton.SetIsVisible(true);
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