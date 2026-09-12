using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapAreaConfig", menuName = "Configs/MapAreaConfig")]
public class MapAreaConfig : ScriptableObject
{
    [SerializeField] private float _radius;

    public event Action<float> RadiusChanged;

    public float Radius => _radius;
    public float BoundSize => CalculateBoundsSize(_radius);

#if UNITY_EDITOR
    private void OnValidate() => RadiusChanged?.Invoke(_radius);
#endif

    public float CalculateBoundsSize(float radius) => radius * 2;

    public bool Contains(Vector3 position) => new Bounds(Vector3.zero, Vector3.one * BoundSize).Contains(position);
}