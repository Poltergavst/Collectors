using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Unit _botPrefab;
    [SerializeField] private int _botsCount  = 3;

    private List<Unit> _availableUnits;

    private PlayerInput _playerInput;

    public event Action<int> UnitsChanged;
    public event Action<int> ResourcesChanged;

    public int ResourceCounter { get; private set; }
    public int BotsCount => _botsCount;

    private void Awake()
    {
        _availableUnits = new();
        _playerInput = new PlayerInput();

        ResourceCounter = 0;

        _playerInput.Game.Scan.performed += _ => SearchForResources();
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void Start() => InstantiateUnits();

    private void InstantiateUnits()
    {
        GameObject container = new("Units");

        for (int i = 0; i < _botsCount; i++)
        {
            Vector3 spawnPosition = PickRandomPosition(i);
            Vector3 lookDirection = Camera.main.transform.position - spawnPosition;
            lookDirection.y = 0f;

            Unit unit = Instantiate(_botPrefab, spawnPosition, Quaternion.LookRotation(lookDirection), container.transform);
            unit.SetBase(this);
            unit.Delivered += OnDelivered;

            _availableUnits.Add(unit);
        }
    }

    private void SearchForResources()
    {
        Dictionary<Unit, IPickable> assignedResources = new();
        List<IPickable> pickables = new();

        foreach (Collider scannedObject in _scanner.Scan(10, transform.position))
        {
            if (scannedObject != null)
            {
                if (scannedObject.gameObject.TryGetComponent<IPickable>(out IPickable pickable))
                {
                    pickables.Add(pickable);
                }
            }
        }

        if (pickables.Count > 0)
        {
            var pairs = new List<(Unit unit, IPickable pickable, float distance)>();

            foreach (Unit unit in _availableUnits)
            {
                foreach (IPickable pickable in pickables)
                {
                    float distance = (pickable.GetCoordinates() - unit.transform.position).sqrMagnitude;
                    pairs.Add((unit, pickable, distance));
                }
            }

            pairs.Sort((pair,nextPair) => pair.distance.CompareTo(nextPair.distance));

            var usedUnits = new HashSet<Unit>();
            var usedPickables = new HashSet<IPickable>();

            foreach (var (unit, pickable, _) in pairs)
            {
                if (usedUnits.Contains(unit) || usedPickables.Contains(pickable))
                    continue;

                usedUnits.Add(unit);
                usedPickables.Add(pickable);

                unit.StartCollection(pickable);
                _availableUnits.Remove(unit);
                UnitsChanged?.Invoke(_availableUnits.Count);

                pickable.GameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    private Vector3 PickRandomPosition(int index)
    {
        float angleStep = Mathf.PI / _botsCount;

        float angle = index * angleStep + angleStep * 0.5f + Mathf.PI * 0.25f;
        float radius = 1f + GetComponent<Collider>().bounds.extents.x;

        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);

        return new Vector3(x, 0, z);
    }

    private void OnDelivered(Unit unit, IPickable item)
    {
        ResourceCounter++;
        ResourcesChanged?.Invoke(ResourceCounter);

        item.Drop();

        _availableUnits.Add(unit);
        UnitsChanged?.Invoke(_availableUnits.Count);
    }
}
