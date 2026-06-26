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

    public void SendGathering(List<IPickable> resources, IReadOnlyCollection<Unit> units, Base homeBase)
    {
        var assignedPairs = _assigner.AssignResources(resources, units);

        foreach (var pair in assignedPairs)
        {
            Assigned?.Invoke(pair.Unit, pair.Pickable);

            StartCollection(pair.Unit, pair.Pickable, homeBase);
        }
    }

    private void StartCollection(Unit unit, IPickable pickable, Base homeBase) => StartCoroutine(CollectResource(unit, pickable, homeBase));

    private IEnumerator CollectResource(Unit unit, IPickable pickable, Base homeBase)
    {
        yield return unit.Collect(pickable, homeBase);
        Gathered?.Invoke(unit, pickable);
    }
}
