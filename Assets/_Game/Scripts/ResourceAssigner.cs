using System.Collections.Generic;

public class ResourceAssigner
{
    private List<(Unit unit, IPickable pickable, float distance)> GatherPairsByDistance(List<IPickable> pickables, IEnumerable<Unit> units)
    {
        var pairs = new List<(Unit unit, IPickable pickable, float distance)>();

        foreach (Unit unit in units)
        {
            foreach (IPickable pickable in pickables)
            {
                float distance = (pickable.GetCoordinates() - unit.transform.position).sqrMagnitude;
                pairs.Add((unit, pickable, distance));
            }
        }

        pairs.Sort((pair, nextPair) => pair.distance.CompareTo(nextPair.distance));

        return pairs;
    }

    public List<(Unit unit, IPickable pickable)> AssignResources(List<IPickable> pickables, IEnumerable<Unit> units)
    {
        var assignedPairs = new List<(Unit unit, IPickable pickable)>();

        var usedUnits = new HashSet<Unit>();
        var usedPickables = new HashSet<IPickable>();

        List<(Unit unit, IPickable pickable, float distance)> pairs = new();

        if (pickables.Count > 0)
        {
            pairs = GatherPairsByDistance(pickables, units);
        }

        foreach (var (unit, pickable, _) in pairs)
        {
            if (usedUnits.Contains(unit) || usedPickables.Contains(pickable))
                continue;

            usedUnits.Add(unit);
            usedPickables.Add(pickable);


            assignedPairs.Add((unit, pickable));
        }

        return assignedPairs;
    }
}
