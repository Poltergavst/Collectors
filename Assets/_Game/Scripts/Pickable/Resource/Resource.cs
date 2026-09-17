using UnityEngine;

public class Resource : MonoBehaviour, IPickable
{
    private ResourceSpawner _spawner;
    public GameObject GameObject => gameObject;

    public Vector3 GetCoordinates() => transform.position;

    public void AssignSpawner(ResourceSpawner spawner) => _spawner = spawner;

    public void Despawn()
    {
        transform.SetParent(_spawner.gameObject.transform);
        _spawner.Release(this);
    }
}
