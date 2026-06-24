using UnityEngine;

public static class Vector3Extension
{
    public static float SqrDistanceTo(this Vector3 start, Vector3 end)
    {
        return (start - end).sqrMagnitude;
    }

    public static bool IsEnoughCloseTo(this Vector3 original, Vector3 otherPoint, float maxDistance)
    {
        return original.SqrDistanceTo(otherPoint) <= maxDistance * maxDistance;
    }

    public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
    {
        return (destination - source).normalized;
    }

    public static Vector3 Change(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x: vector.x + (x ?? 0), y: vector.y + (y ?? 0), z: vector.z + (z ?? 0));
    }
}