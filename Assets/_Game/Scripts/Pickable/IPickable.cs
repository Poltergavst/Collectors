using UnityEngine;

public interface IPickable
{
    public GameObject GameObject { get; }

    public void Despawn();
    public Vector3 GetCoordinates();
}