using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;

public partial class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Collider _base;
    [SerializeField] private Resource[] _prefabs;
    [SerializeField] private float _radius = 10;
    [SerializeField] private float _exclusionRadius = 1;
    [SerializeField] private int _maxToSpawn = 10;

    private int _index;
    private List<Vector2> _points;
    private Dictionary<Resource, Vector3> _occupiedPoints;
    private HashSet<Vector3> _occupiedPositions;

    private ObjectPool<Resource> _pool;

    private void Start()
    {
        SetUpPoints();
        _occupiedPoints = new();
        _occupiedPositions = new();

        _maxToSpawn = Mathf.Min(_points.Count, _maxToSpawn);

        _pool = new ObjectPool<Resource>(
            createFunc: Spawn,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            maxSize: _maxToSpawn
        );
        
        InvokeRepeating(nameof(GetFromPool), 1f, 2f);
    }

    public void GetFromPool()
    {
        if (_pool.CountActive < _maxToSpawn)
        {
            _pool.Get();
        }
    }

    public void Release(Resource resource)
    {
        _pool.Release(resource);
    }

    private void OnGet(Resource resource)
    {
        int tries = 0;

        Vector3 position = PickRandomPosition();

        while (_occupiedPositions.Contains(position) && tries <= _maxToSpawn)
        {
            position = PickRandomPosition();

            tries++;
        }

        resource.transform.position = position;

        _occupiedPositions.Add(position);
        _occupiedPoints.Add(resource, position);

        resource.gameObject.SetActive(true);
        resource.GetComponent<Collider>().enabled = true;
    }
    
    private void OnRelease(Resource resource)
    {
        if (_occupiedPoints.TryGetValue(resource, out Vector3 position))
        {
            _occupiedPoints.Remove(resource);
            _occupiedPositions.Remove(position);
        }

        resource.gameObject.SetActive(false);
    }

    private Resource Spawn()
    {
        if (_prefabs.Length == 0)
        {
            return null;
        }

        Resource prefab = _prefabs[Random.Range(0, _prefabs.Length)];

        Resource resource = Instantiate(prefab, PickRandomPosition(), Quaternion.identity, transform);
        resource.AssignSpawner(this);

        return resource;
    }

    private void SetUpPoints()
    {
        Vector2 regionSize = Vector2.one * (_radius * 2);
        Vector2 regionOrigin = new Vector2(transform.position.x, transform.position.z);
        Vector2 excludeCenter = regionOrigin;

        _points = PoissonDiscSampling.GeneratePoints(1.5f, regionSize, 20);

        for (int i = 0; i < _points.Count; i++)
            _points[i] = regionOrigin - regionSize * 0.5f + _points[i];

        _points.RemoveAll(p => 
        (p - excludeCenter).sqrMagnitude < _exclusionRadius * _exclusionRadius || 
        (p - excludeCenter).sqrMagnitude > _radius * _radius);

        Shuffle(ref _points);

        _index = _points.Count;
    }

    private Vector3 PickRandomPosition()
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

    private void Shuffle(ref List<Vector2> points)
    {
        for (int i = points.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (points[i], points[j]) = (points[j], points[i]);
        }
    }
}
