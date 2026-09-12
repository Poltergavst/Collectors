using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnDistance;
    [SerializeField] private Unit _unitPrefab;

    private GameObject _container;
    private Vector3 _spawnPosition;
    private Vector3 _lookDirection;
    private UnitsRegistry _units;

    public void Initialise(UnitsRegistry units)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        
        _units = units;
        _container = new("Units");

        _spawnPosition = GetSpawnPosition(cameraPosition);
        _lookDirection = (cameraPosition - _spawnPosition).Change(y: 0);
    }

    public void SpawnSeveral(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Unit newUnit = Instantiate(_unitPrefab, _spawnPosition, Quaternion.LookRotation(_lookDirection), _container.transform);
        _units.Register(newUnit);
    }

    private Vector3 GetSpawnPosition(Vector3 cameraPosition)
    {
        Vector3 directionToCamera = transform.position.DirectionTo(cameraPosition);

        float angle = Mathf.Atan2(directionToCamera.z, directionToCamera.x);

        float x = _spawnDistance * Mathf.Cos(angle);
        float z = _spawnDistance * Mathf.Sin(angle);

        return transform.position + new Vector3(x, 0, z);
    }
}
