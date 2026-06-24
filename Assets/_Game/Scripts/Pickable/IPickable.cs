using UnityEngine;

public interface IPickable
{
    public GameObject GameObject { get; }

    public Vector3 GetCoordinates();
    public void Drop(Vector3 position);
    public YieldInstruction PickUp(Transform container);

    public void EnableForDetection();
    public void DisableForDetection();
}