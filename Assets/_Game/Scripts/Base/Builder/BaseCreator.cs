using UnityEngine;

public class BaseCreator
{
    private SharedBaseServices _shared;

    public BaseCreator(SharedBaseServices shared)
    {
        _shared = shared;
    }

    public Base Create(Vector3 position)
    {
        var instance = Object.Instantiate(_shared.BasePrefab, position.Change(y: 0.1f), Quaternion.identity);

        instance.ConfigureSharedServices(_shared);
        _shared.SpawnpointsProvider.AddExclusionZone(instance.GetComponent<Collider>());

        return instance;
    }
}