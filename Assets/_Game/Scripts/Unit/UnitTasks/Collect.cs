using System.Collections;

public class Collect : IUnitTask
{  
    private IPickable _pickable;

    public Collect(IPickable pickable) => _pickable = pickable;

    public IEnumerator Execute(Unit unit)
    {
        yield return unit.Carrier.PickUp(_pickable);
    }
}
