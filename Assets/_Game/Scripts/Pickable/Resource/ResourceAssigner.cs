using System.Collections.Generic;

public class ResourceAssigner
{
    public List<CollectionPair> AssignResources(List<IPickable> pickables, IEnumerable<Unit> units)
    {
        var usedUnits = new HashSet<Unit>();
        var usedPickables = new HashSet<IPickable>();
        var assignedPairs = new List<CollectionPair>();

        List<CollectionPair> candidates = new();

        if (pickables.Count > 0)
        {
            candidates = GatherPairsByDistance(pickables, units);
        }

        foreach (var candidate in candidates)
        {
            if (usedUnits.Contains(candidate.Unit) || usedPickables.Contains(candidate.Pickable))
                continue;

            usedUnits.Add(candidate.Unit);
            usedPickables.Add(candidate.Pickable);
            assignedPairs.Add(candidate);
        }

        return assignedPairs;
    }

    private List<CollectionPair> GatherPairsByDistance(List<IPickable> pickables, IEnumerable<Unit> units)
    {
        var pairs = new List<CollectionPair>();

        foreach (Unit unit in units)
            foreach (IPickable pickable in pickables)
                pairs.Add(new(unit, pickable));

        pairs.Sort((pair, nextPair) => pair.SqrDistance.CompareTo(nextPair.SqrDistance));

        return pairs;
    }
}
