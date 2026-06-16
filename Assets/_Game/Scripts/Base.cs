using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitSpawner), typeof(ResourceGatherer))]
public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;

    private UnitPool _unitPool;
    private UnitSpawner _unitSpawner;
    private ResourceGatherer _resourceGatherer;

    public event Action<int> UnitsCountChanged;
    public event Action<int> ResourcesCountChanged;

    public int ResourceCounter { get; private set; }
    public int UnitsCapacity => _unitSpawner.Units.Capacity;

    private void Awake()
    {
        ResourceCounter = 0;

        _unitSpawner = GetComponent<UnitSpawner>();
        _resourceGatherer = GetComponent<ResourceGatherer>();
    }

    private void OnEnable() => Subscribe();
    private void OnDisable() => Unsubscribe();

    private void Start()
    {
        _unitSpawner.InstantiateUnits();
        _unitPool = _unitSpawner.Units;
    }

    private void FilterScanned(Collider[] scannedObjects)
    {
        if (scannedObjects.Length == 0) return;

        List<IPickable> pickables = new();

        foreach (Collider scannedObject in scannedObjects)
        {
            if (scannedObject != null && scannedObject.gameObject.TryGetComponent(out IPickable pickable))
            {
                pickables.Add(pickable);
            }
        }

        _resourceGatherer.SendGathering(pickables, _unitPool.GetUnits(), this);
    }

    private void OnAssigned(Unit unit, IPickable pickable)
    {
        _unitPool.Release(unit);
        pickable.DisableForDetection();

        UnitsCountChanged?.Invoke(_unitPool.Count);
    }

    private void OnDelivered(Unit unit, IPickable pickable)
    {
        ResourceCounter++;

        _unitPool.Return(unit);
        pickable.Drop(transform.position);

        ResourcesCountChanged?.Invoke(ResourceCounter);
        UnitsCountChanged?.Invoke(_unitPool.Count);
    }

    private void Subscribe()
    {
        _scanner.ScanPerformed += FilterScanned;
        _resourceGatherer.Gathered += OnDelivered;
        _resourceGatherer.Assigned += OnAssigned;
    }

    private void Unsubscribe()
    {
        _scanner.ScanPerformed -= FilterScanned;
        _resourceGatherer.Gathered -= OnDelivered;
        _resourceGatherer.Assigned -= OnAssigned;
    }
}