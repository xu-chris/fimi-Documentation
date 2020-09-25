using _Project.Scripts.DomainObjects;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public sealed class SkeletonOrchestrator : ISkeletonOrchestrator
    {
        private readonly int maxNumberOfPeople;

        private Skeleton[] skeletons;

        public SkeletonOrchestrator(int maxNumberOfPeople)
        {
            this.maxNumberOfPeople = maxNumberOfPeople;
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
                    skeletons[p] = new Skeleton(p);
                    Debug.LogError("Initialized a new skeleton which should be already there 🤔. p: " + p);
                }

                // Set and activate only skeletons that are detected.
                if (p >= 0 && detectedPersons.Length > p && p == detectedPersons[p].id)
                    UpdateSkeleton(skeletons[p], detectedPersons[p]);
                else
                    skeletons[p].SetIsVisible(false);
            }
        }

        public void InitializeAllSkeletons()
        {
            skeletons = new Skeleton[maxNumberOfPeople];
            for (var p = 0; p < maxNumberOfPeople; p++)
            {
                skeletons[p] = new Skeleton(p);
                skeletons[p].SetIsVisible(false);
            }
        }

        private void UpdateSkeleton(Skeleton skeleton, Person person)
        {
            skeleton.SetSkeleton(person.joints, person.lowestY);
            skeleton.SetIsVisible(true);
        }
    }
}