using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class SkeletonOrchestrator
    {
        private readonly GameObject _plane;
        private Skeleton[] _skeletons;
        private List<int> _validJointIdx;
        private const int _maxNumberOfPeople = 10;

        public SkeletonOrchestrator()
        {
            _plane = GameObject.Find("CheckerboardPlane");
            InitializeAllSkeletons();
        }

        public void Update(Person[] detectedPersons, float lowestY)
        {
            if (detectedPersons == null)
                return;

            for (var p = 0; p < _maxNumberOfPeople; p++)
            {
                // Init skeleton if not given.
                if (_skeletons[p] == null)
                {
                    _skeletons[p] = new Skeleton(p);
                    Debug.LogError("Initialized a new skeleton which should be already there 🤔. p: " + p);
                }

                // Set and activate only skeletons that are detected.
                if (p >= 0 && detectedPersons.Length > p && p == detectedPersons[p].ID)
                {
                    _skeletons[p].SetSkeleton(detectedPersons[p].Joints, _plane, lowestY);
                    _skeletons[p].SetIsVisible(true);
                    Debug.Log("Set skeleton " + p + " and set it to visible.");
                }
                else
                {
                    _skeletons[p].SetIsVisible(false);
                    Debug.Log("Set skeleton " + p + " to invisible (not active).");
                }
            }
        }

        private void InitializeAllSkeletons()
        {
            _skeletons = new Skeleton[_maxNumberOfPeople];
            for (var p = 0; p < _maxNumberOfPeople; p++)
            {
                _skeletons[p] = new Skeleton(p);
                _skeletons[p].SetIsVisible(false);
            }
        }
    }
}