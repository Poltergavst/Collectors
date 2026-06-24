#if UNITY_EDITOR
using UnityEngine;

public partial class ResourceSpawner
{
    [SerializeField] private float _pointDisplayRadius = 1;

    void OnValidate()
    {
        _radius = Mathf.Max(0, _radius);
        _maxToSpawn = Mathf.Max(0, _maxToSpawn);

        if (_base != null)
            SetUpPoints();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            new Vector3(transform.position.x, 0, transform.position.z), 
            new Vector3(_radius, 0, _radius) * 2
            );

        foreach (var zone in _exclusionZones)
        {
            Bounds bounds = zone.bounds;

            bounds.Expand(_exclusionExpansion);

            Gizmos.color = Color.red;
            DrawWireCircle(transform.position, bounds.extents.x);
        }

        Gizmos.color = Color.green;
        DrawWireCircle(transform.position, _radius);

        if (_points != null)
        {
            foreach (Vector2 point in _points)
            {
                Vector3 worldPosition = new (point.x, transform.position.y, point.y);
                Gizmos.DrawSphere(worldPosition, _pointDisplayRadius);
            }
        }
    }

    private void DrawWireCircle(Vector3 center, float radius, int segments = 12)
    {
        float fullCircle = 2 * Mathf.PI;
        float step = fullCircle / segments;

        Vector3 previous = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * step;

            Vector3 next = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            Gizmos.DrawLine(previous, next);
            previous = next;
        }
    }
}
#endif