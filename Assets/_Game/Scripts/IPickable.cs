using UnityEngine;

public interface IPickable
{
    public Vector3 GetCoordinates();
    public GameObject GameObject { get; }

    public void Drop(Vector3 position);
    public YieldInstruction PickUp(Transform container);

    public void EnableForDetection();
    public void DisableForDetection();
}