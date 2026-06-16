using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    private ResourceAssigner _assigner;

    public event Action<Unit, IPickable> Assigned;
    public event Action<Unit, IPickable> Gathered;

    private void Awake()
    {
        _assigner = new();
    }

    public void SendGathering(List<IPickable> rersources, IReadOnlyCollection<Unit> units, Base homeBase)
    {
        var assignedPairs = _assigner.AssignResources(rersources, units);

        foreach (var (unit, pickable) in assignedPairs)
        {
            Assigned?.Invoke(unit, pickable);

            StartCollection(unit, pickable, homeBase);
        }
    }

    private void StartCollection(Unit unit, IPickable pickable, Base homeBase)
    {
        StartCoroutine(CollectResource(unit, pickable, homeBase));
    }

    private IEnumerator CollectResource(Unit unit, IPickable pickable, Base homeBase)
    {
        yield return unit.NavigateTo(pickable.GetCoordinates());
        yield return pickable.PickUp(unit.Hand);
        yield return unit.ReturnToBase(homeBase);

        Gathered?.Invoke(unit, pickable);
    }
}
