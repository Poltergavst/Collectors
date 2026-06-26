using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private int _spawnAmount;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private Unit _unitPrefab;

    public UnitsRegistry Units { get; private set; }

    public void InstantiateUnits()
    {
        GameObject container = new("Units");
        List<Unit> units = new();

        Vector3 cameraPosition = Camera.main.transform.position;
        float startAngle = GetInitialSpawnAngle(cameraPosition);

        for (int i = 0; i < _spawnAmount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(i, startAngle);
            Vector3 lookDirection = (cameraPosition - spawnPosition).Change(y: 0);

            Unit newUnit = Instantiate(_unitPrefab, spawnPosition, Quaternion.LookRotation(lookDirection), container.transform);

            units.Add(newUnit);
        }

        Units = new(units);
    }

    private float GetInitialSpawnAngle(Vector3 cameraPosition)
    {
        Vector3 directionToCamera = transform.position.DirectionTo(cameraPosition);

        float baseAngle = Mathf.Atan2(directionToCamera.z, directionToCamera.x);
        
        return baseAngle - Mathf.PI * MathConstants.Half;
    }

    private Vector3 GetSpawnPosition(int index, float startAngle)
    {
        float step = Mathf.PI / _spawnAmount;
        float angle = startAngle + step * MathConstants.Half + index * step;

        float x = _spawnDistance * Mathf.Cos(angle);
        float z = _spawnDistance * Mathf.Sin(angle);

        return new Vector3(x, 0, z);
    }
}
