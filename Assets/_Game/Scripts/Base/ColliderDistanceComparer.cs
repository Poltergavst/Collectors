using System.Collections.Generic;
using UnityEngine;

internal class ColliderDistanceComparer : Comparer<Collider>
{
    private Vector3 _position;

    public void SetPoistion(Vector3 position)
    {
        _position = position;
    }

    public override int Compare(Collider x, Collider y)
    {
        float xMagnitude = _position.SqrDistanceTo(x.transform.position);
        float yMagnitude = _position.SqrDistanceTo(y.transform.position);

        return xMagnitude.CompareTo(yMagnitude);
    }
}