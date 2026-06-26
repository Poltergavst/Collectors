using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SpawnpointsProvider))]
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private int _maxToSpawn = 10;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private Resource[] _prefabs;
    [SerializeField] private SpawnpointsProvider _spawnpointsProvider;

    private ObjectPool<Resource> _pool;
    private Dictionary<Resource, Vector3> _occupiedPoints;

    private void Start()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: Spawn,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            maxSize: _maxToSpawn
        );

        _occupiedPoints = new();
        _spawnpointsProvider = GetComponent<SpawnpointsProvider>();

        StartCoroutine(SpawnRoutine());
    }

    public void GetFromPool()
    {
        if (_pool.CountActive < _maxToSpawn)
            _pool.Get();
    }

    public void Release(Resource resource) => _pool.Release(resource);

    private void OnGet(Resource resource)
    {
        Vector3 position = _spawnpointsProvider.GetSpawnPosition(_maxToSpawn);

        _occupiedPoints.Add(resource, position);

        resource.transform.position = position;
        resource.gameObject.SetActive(true);
        resource.EnableForDetection();
    }
    
    private void OnRelease(Resource resource)
    {
        if (_occupiedPoints.TryGetValue(resource, out Vector3 position))
        {
            _occupiedPoints.Remove(resource);
            _spawnpointsProvider.ReleasePoint(position);
        }

        resource.gameObject.SetActive(false);
    }

    private Resource Spawn()
    {
        if (_prefabs.Length == 0)
            return null;

        Resource prefab = _prefabs[Random.Range(0, _prefabs.Length)];
        Resource resource = Instantiate(prefab, transform);
        resource.AssignSpawner(this);

        return resource;
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            GetFromPool();
            yield return wait;
        }
    }
}
