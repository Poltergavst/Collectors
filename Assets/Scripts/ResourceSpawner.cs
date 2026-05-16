using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Collider _base;
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _radius = 10;
    [SerializeField] private int _maxSpawned = 10;

    private float _forbiddenRadius;

    private ObjectPool<Resource> _pool;

    private void Start()
    {
        _forbiddenRadius = new Vector2(_base.bounds.extents.x, _base.bounds.extents.z).magnitude;

        _pool = new ObjectPool<Resource>(
            createFunc: Spawn,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            maxSize: 10
        );
        
        InvokeRepeating(nameof(GetFromPool), 1f, 2f);
    }

    public void GetFromPool()
    {
        if (_pool.CountActive < 10)
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
        resource.transform.position = PickRandomPosition();
        resource.gameObject.SetActive(true);
    }
    
    private void OnRelease(Resource resource)
    {
        resource.gameObject.SetActive(false);
    }

    private Resource Spawn()
    {
        Resource resource = Instantiate(_prefab, PickRandomPosition(), Quaternion.identity, transform);
        resource.AssignSpawner(this);

        return resource;
    }

    private Vector3 PickRandomPosition()
    {
        // 1. Получаем направление (радианы)
        float angle = Random.value * 2 * Mathf.PI;

        // 2. Получаем радиус (линейное распределение)
        // Точки будут плотнее у minRadius и реже у maxRadius
        float radius = Random.Range(_forbiddenRadius, _radius);

        // 3. Переводим в декартовы координаты
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);

        return new Vector3(x, 0, z);
    }
}
