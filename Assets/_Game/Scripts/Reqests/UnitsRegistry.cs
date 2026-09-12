using System;
using System.Collections.Generic;
using System.Linq;

public class UnitsRegistry : RequestMaker<UnitRequest>
{
    private HashSet<Unit> _availableUnits = new();

    public event Action<int> UnitsCountChanged;

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

        Capacity = _availableUnits.Count;
        UnitsCountChanged?.Invoke(Count);
    }

    public void Register(Unit unit)
    {
        Capacity++;
        UnitsCountChanged?.Invoke(Count);
        Return(unit);
    }

    public void Return(Unit unit)
    {
        _availableUnits.Add(unit);
        UnitsCountChanged?.Invoke(Count);
    }

    public bool TryRelease(Unit unit) 
    {
        if (IsEnough(1))
        {
            _availableUnits.Remove(unit);
            UnitsCountChanged?.Invoke(Count);

            return true;
        }

        return false;
    }

    public bool TryGiveUnits(int amount, out List<Unit> units)
    {
        const int minToKeep = 1;
        units = new();

        if (IsEnough(amount) && Capacity > minToKeep)
        {
            for (int i = 0; i < amount; i++)
            { 
               units.Add(GiveUnit());
            }

            return true;
        }

        return false;
    }

    public override void MakeRequest(UnitRequest request)
    {
        AddToRequests(request);
    }

    protected override bool TryFulfill(UnitRequest request)
    {
        if (TryGiveUnits(request.Required, out List<Unit> units))
        {
            request.Callback?.Invoke(units);
            return true;
        }

        return false;
    }

    protected override bool IsEnough(int amount) => Count >= amount;

    private Unit GiveUnit()
    {
        Unit unit = _availableUnits.FirstOrDefault();

        if (TryRelease(unit))
        { 
            Capacity--;
            UnitsCountChanged?.Invoke(Count);
        }

        return unit;
    }
}
