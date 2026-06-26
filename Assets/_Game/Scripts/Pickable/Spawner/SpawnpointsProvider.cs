using System.Collections.Generic;
using UnityEngine;

public partial class SpawnpointsProvider : MonoBehaviour
{
    [SerializeField] private Collider _base;
    [SerializeField] private float _spaceBetweenPoints = 2f;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _exclusionExpansion = 1f;
    [SerializeField] private Collider[] _exclusionZones;

    private List<Vector2> _points;
    private HashSet<Vector3> _occupiedPositions;

    private int _index;

    private void Awake()
    {
        _occupiedPositions = new();
        SetUpPoints();
    }

    public void ReleasePoint(Vector3 position)
    {
        _occupiedPositions.Remove(position);
    }

    public Vector3 GetSpawnPosition(int maxToSpawn)
    {
        int tries = 0;
        Vector3 position = PickNextPosition();

        while (_occupiedPositions.Contains(position) && tries <= maxToSpawn)
        {
            position = PickNextPosition();
            tries++;
        }

        _occupiedPositions.Add(position);

        return position;
    }

    private void SetUpPoints()
    {
        Vector2 regionSize = Vector2.one * (_radius * 2);
        Vector2 regionOrigin = new(transform.position.x, transform.position.z);

        _points = PoissonDiscSampling.GeneratePoints(_spaceBetweenPoints, regionSize);

        for (int i = 0; i < _points.Count; i++)
            _points[i] = regionOrigin - regionSize * MathConstants.Half + _points[i];

        _points.RemoveAll(point => MustBeExcluded(point, regionOrigin));

        Shuffle(_points);

        _index = _points.Count;
    }

    private bool MustBeExcluded(Vector2 p, Vector2 regionOrigin)
    {
        Vector3 worldPoint = new(p.x, 0, p.y);

        foreach (var zone in _exclusionZones)
        {
            Bounds bounds = zone.bounds;

            bounds.Expand(_exclusionExpansion);

            bool isInsideCircle = bounds.center.SqrDistanceTo(worldPoint) < bounds.extents.x * bounds.extents.x;

            if (bounds.Contains(worldPoint) && isInsideCircle)
                return true;
        }

        return regionOrigin.SqrDistanceTo(p) > _radius * _radius;
    }

    private void Shuffle(List<Vector2> points)
    {
        Vector3 temp;

        for (int i = points.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            temp = points[i];

            points[i] = points[j];
            points[j] = temp;
        }
    }

    private Vector3 PickNextPosition()
    {
        _index--;

        if (_index < 0)
        {
            _index = _points.Count - 1;
        }

        float x = _points[_index].x;
        float z = _points[_index].y;

        return transform.position + new Vector3(x, 0, z);
    }
}
