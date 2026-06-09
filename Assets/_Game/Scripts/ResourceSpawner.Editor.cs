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
            new Vector3(_radius * 2, 0, _radius * 2)
            );

        Gizmos.color = Color.red;
        DrawWireCircle(transform.position, _exclusionRadius);

        Gizmos.color = Color.green;
        DrawWireCircle(transform.position, _radius);

        if (_points != null)
        {
            foreach (Vector2 point in _points)
            {
                Vector3 worldPosition = new Vector3(point.x, transform.position.y, point.y);
                Gizmos.DrawSphere(worldPosition, _pointDisplayRadius);
            }
        }
    }

    private void DrawWireCircle(Vector3 center, float radius, int segments = 16)
    {
        float fullCircle = 2 * Mathf.PI;
        float step = fullCircle / segments;

        Vector3 prev = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * step;
            Vector3 next = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
#endif