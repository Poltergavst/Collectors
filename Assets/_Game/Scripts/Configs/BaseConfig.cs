using UnityEngine;

[CreateAssetMenu(fileName = "BaseConfig", menuName = "Configs/BaseConfig")]
public class BaseConfig : ScriptableObject
{
    [Header("Building")]
    [SerializeField] private int _buildingPrice = 5;
    [SerializeField] private float _requiredSpaceRadius = 7f;

    [Header("UnitCreation")]
    [SerializeField] private int _initialUnitCount = 1;
    [SerializeField] private int _creationPrice = 3;
    [SerializeField] private float _creationTime = 0.2f;

    public int BuildingPrice => _buildingPrice;
    public float RequiredSpaceRadius => _requiredSpaceRadius;
    public int CreationPrice  => _creationPrice;
    public float CreationTime => _creationTime;
    public int InitialUnitCount => _initialUnitCount;
}