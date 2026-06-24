using UnityEngine;

public static class Vector2Extension
{
    public static float SqrDistanceTo(this Vector2 start, Vector2 end)
    {
        return (start - end).sqrMagnitude;
    }

    public static bool IsEnoughCloseTo(this Vector2 original, Vector2 otherPoint, float maxDistance)
    {
        return original.SqrDistanceTo(otherPoint) <= maxDistance * maxDistance;
    }

    public static Vector2 DirectionTo(this Vector2 source, Vector2 destination)
    {
        return (destination - source).normalized;
    }

    public static Vector2 Change(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vector.x, y ?? vector.y);
    }

    public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(x: vector.x + (x ?? 0), y: vector.y + (y ?? 0));
    }
}
