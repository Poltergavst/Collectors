using System.Collections.Generic;

public class UnitsRegistry
{
    private HashSet<Unit> _availableUnits = new();

    public UnitsRegistry(IEnumerable<Unit> spawnedUnits)
    {
        Set(spawnedUnits);
    }

    public int Capacity { get; private set; } = 0;
    public int Count => _availableUnits.Count;
    public IReadOnlyCollection<Unit> Units => _availableUnits;

    public void Set(IEnumerable<Unit> spawnedUnits)
    {
        _availableUnits.Clear();

        foreach (var unit in spawnedUnits)
        {
            Register(unit);
        }
    }

    public void Register(Unit unit)
    {
        Return(unit);
        Capacity++;
    }

    public void Return(Unit unit)
    {
        _availableUnits.Add(unit);
    }

    public void Release(Unit unit) 
    {
        _availableUnits.Remove(unit);
    }
}
