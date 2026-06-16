using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private float spawnDistance;

    public UnitPool Units { get; private set; }

    public void InstantiateUnits()
    {
        GameObject container = new("Units");
        List<Unit> units = new();

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(i);
            Vector3 lookDirection = (Camera.main.transform.position - spawnPosition).Change(y: 0);

            units.Add(Instantiate(_unitPrefab, spawnPosition, Quaternion.LookRotation(lookDirection), container.transform));
        }

        Units = new(units);
    }

    private Vector3 GetSpawnPosition(int index)
    {
        float angleStep = Mathf.PI / spawnAmount;

        float angle = index * angleStep + angleStep * 0.5f + Mathf.PI * 0.25f;
        float radius = spawnDistance;

        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);

        return new Vector3(x, 0, z);
    }
}
