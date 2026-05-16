using UnityEngine;

public class Resource : MonoBehaviour, IPickable
{
    public GameObject GameObject => gameObject;

    private ResourceSpawner _spawner;

    public Vector3 GetCoordinates()
    {
        return transform.position;
    }

    public void PickUp(Transform container)
    {
        transform.position = container.position;
        transform.SetParent(container);
    }

    public void Drop()
    {
        //transform.position = dropPosition;

        //_spawner.RemoveFromSpawned();

        transform.SetParent(_spawner.gameObject.transform);
        _spawner.Release(this);
    }

    public void AssignSpawner(ResourceSpawner spawner)
    {
        _spawner = spawner;
    }
}
