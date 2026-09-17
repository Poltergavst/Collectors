using System;
using UnityEngine;

public class ResourceGatherer
{
    private ResourceAssigner _assigner;

    public event Action<Unit, IPickable> Gathered;

    public ResourceGatherer()
    {
        _assigner = new();
    }

    public void SendGathering(ResourceRepository repository, UnitsRegistry units, Transform baseTransform, float baseRadius)
    {
        var assignedPairs = _assigner.AssignResources(repository.FreeResources, units.Units);

        foreach (var pair in assignedPairs)
        {
            if (units.TryRelease(pair.Unit))
            {
                repository.AddToTaken(pair.Pickable);
                SendUnit(pair.Unit, pair.Pickable, baseTransform, baseRadius);
            }
        }
    }

    private void SendUnit(Unit unit, IPickable pickable, Transform baseTransform, float baseRadius)
    {
        float stoppingDistance = 1.25f;

        unit.TasksCompleted += OnCompletion;

        unit.AddTaskToQueue(new ReachTarget(pickable.GameObject.transform, stoppingDistance));
        unit.AddTaskToQueue(new Collect(pickable));
        unit.AddTaskToQueue(new ReachTarget(baseTransform, baseRadius + stoppingDistance));
        unit.AddTaskToQueue(new Drop(baseTransform.position));

        unit.PerformTasks();

        void OnCompletion()
        {
            Gathered?.Invoke(unit, pickable);
            unit.TasksCompleted -= OnCompletion;
        }
    }
}
