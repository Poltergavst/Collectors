using UnityEngine;

public class Resource : MonoBehaviour, IPickable
{
    private ResourceSpawner _spawner;

    public bool IsDetectable { get; private set; } = true;
    public GameObject GameObject => gameObject;

    public Vector3 GetCoordinates() => transform.position;

    public void AssignSpawner(ResourceSpawner spawner) => _spawner = spawner;

    public void Despawn()
    {
        transform.SetParent(_spawner.gameObject.transform);
        _spawner.Release(this);
    }

    public void DisableForDetection() => IsDetectable = false;

    public void EnableForDetection() => IsDetectable = true;
}
