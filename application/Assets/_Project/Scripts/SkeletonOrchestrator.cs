using System.Collections.Generic;
using UnityEngine;

public class SkeletonOrchestrator
{
    private GameObject _plane;
    private Skeleton[] _skeletons;
    private List<int> _validJointIdx;
    private readonly int maxNumberOfPeople = 10;

    public void Start()
    {
        _plane = GameObject.Find("CheckerboardPlane");
        InitializeAllSkeletons();
    }

    public void Update(Person[] detectedPersons, float lowestY)
    {
        if (detectedPersons == null)
            return;

        for (var p = 0; p < detectedPersons.Length; ++p)
        {
            _skeletons[p].SetSkeleton(detectedPersons[p].Joints, _plane, lowestY);
            _skeletons[p].SetIsVisible(true);
        }

        for (var p = detectedPersons.Length; p < maxNumberOfPeople; ++p) _skeletons[p].SetIsVisible(false);
    }

    private void InitializeAllSkeletons()
    {
        _skeletons = new Skeleton[maxNumberOfPeople];

        for (var p = 0; p < maxNumberOfPeople; ++p) _skeletons[p] = new Skeleton();
    }
}