using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour, IPickable
{
    private Collider _collider;
    private ResourceSpawner _spawner;

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public Vector3 GetCoordinates()
    {
        return transform.position;
    }

    public void AssignSpawner(ResourceSpawner spawner)
    {
        _spawner = spawner;
    }

    public void Despawn()
    {
        transform.SetParent(_spawner.gameObject.transform);
        _spawner.Release(this);
    }

    public void DisableForDetection()
    {
        _collider.enabled = false;
    }

    public void EnableForDetection()
    {
        _collider.enabled = true;
    }
}
