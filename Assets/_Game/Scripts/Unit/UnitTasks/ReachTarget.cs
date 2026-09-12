using System.Collections;
using UnityEngine;

public class ReachTarget : IUnitTask
{
    private Transform _target;
    private float _stoppingDistance;

    public ReachTarget(Transform target, float stoppingDistance)
    {
        _target = target;
        _stoppingDistance = stoppingDistance;
    }

    public IEnumerator Execute(Unit unit)
    {
        yield return unit.Reacher.ReachTarget(_target, _stoppingDistance);
    }
}
