using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Base : MonoBehaviour
{
    [SerializeField] private int _unitsCount  = 3;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Unit _unitPrefab;

    private Queue<Unit> _availableUnits;
    private int _resourceCounter = 0;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _availableUnits = new();

        _playerInput.Game.Scan.performed += context => SearchForResources();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Start()
    {
        InstantiateUnits();
    }

    private void InstantiateUnits()
    {
        GameObject container = new("Units");

        for(int i = 0; i < _unitsCount; i++)
        {
            Unit createdUnit = Instantiate(_unitPrefab, transform.position, Quaternion.identity, container.transform);
            createdUnit.SetBase(transform);
            //createdUnit.gameObject.SetActive(false);
            createdUnit.Delivered += OnDelivered;
            _availableUnits.Enqueue(createdUnit);
        }
    }

    private void SearchForResources()
    {
        int index = 0;
        int maxIndex = _availableUnits.Count;

        foreach (Collider scannedObject in _scanner.Scan(10))
        {
            if (index >= maxIndex)
                return;

            if(scannedObject != null)
            {
                if (scannedObject.gameObject.TryGetComponent<IPickable>(out IPickable pickable))
                {
                    Unit pickedUnit = _availableUnits.Dequeue();

                    //либо так либо убирать их нафик из списка пока не доставят ресурс
                    //if (pickedUnit.IsDelivering == false)
                    //{
                    //    //pickedUnit.gameObject.SetActive(true);
                    //    pickedUnit.StartCollection(pickable);
                    //}

                    pickedUnit.StartCollection(pickable);

                    pickable.GameObject.GetComponent<Collider>().enabled = false;

                    index++;
                }
            }
        }
    }

    private void OnDelivered(Unit unit, IPickable item)
    {
        _resourceCounter++;
        Debug.Log(_resourceCounter);
        item.GameObject.GetComponent<Collider>().enabled = true;
        item.Drop();
        _availableUnits.Enqueue(unit);
    }
}
