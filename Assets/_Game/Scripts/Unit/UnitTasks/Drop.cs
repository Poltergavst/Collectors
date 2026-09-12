using System.Collections;
using UnityEngine;

public class Drop : IUnitTask
{
    private Vector3 _dropPosition;

    public Drop(Vector3 dropPosition) => _dropPosition = dropPosition;

    public IEnumerator Execute(Unit unit)
    {
        unit.Carrier.Drop(_dropPosition);
        yield return null;
    }
}
