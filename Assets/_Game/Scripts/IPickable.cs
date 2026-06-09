using UnityEngine;

public interface IPickable
{
    public GameObject GameObject { get; }

    public Vector3 GetCoordinates();

    public YieldInstruction PickUp(Transform container);

    public void Drop();
}