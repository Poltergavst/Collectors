using UnityEngine;

[CreateAssetMenu(fileName = "CarrierAnimationsConfig", menuName = "Configs/CarrierAnimationsConfig")]
public class CarrierAnimationsConfig : ScriptableObject
{
    [SerializeField] private float _pickUpDuration;
    [SerializeField] private float _pickUpHeight;
    [SerializeField] private float _dropDuration;
    [SerializeField] private float _dropHeight;

    public float PickUpDuration => _pickUpDuration;
    public float PickUpHeight => _pickUpHeight;
    public float DropDuration => _dropDuration;
    public float DropHeight => _dropHeight;
}