using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scanner : MonoBehaviour
{
    [SerializeField] private int _limit = 10;
    [SerializeField] private float _radius = 20f;
    [SerializeField] private LayerMask _layersToScan;
    [Header("----------------")]
    [SerializeField, Tooltip("При выкл. сканирование на Q")] 
    private bool _isTimed;
    [SerializeField] private int _interval = 5;

    private PlayerInput _playerInput;
    private Coroutine _scanCoroutine;

    public event Action<Collider[]> ScanPerformed;

    private void OnValidate()
    {
        _interval = Mathf.Max(0, _interval);

        if (Application.isPlaying == false)
            return;

        if (_scanCoroutine == null && _isTimed)
        {
            _scanCoroutine = StartCoroutine(PerformIntervaledScan());
        }
        else if (_scanCoroutine != null && _isTimed == false)
        {
            StopCoroutine(_scanCoroutine);
            _scanCoroutine = null;
        }
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Game.Scan.performed += OnScanInputPerformed;
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void Start()
    {
        if (_isTimed)
        {
            _scanCoroutine = StartCoroutine(PerformIntervaledScan());
        }
    }

    private void OnDestroy() => _playerInput.Game.Scan.performed -= OnScanInputPerformed;

    public Collider[] Scan(Vector3 position)
    {
        Collider[] results = new Collider[_limit];

        Physics.OverlapSphereNonAlloc(position, _radius, results, _layersToScan);

        results = results.Where(collider => collider != null).OrderBy(collider => (collider.transform.position - position).sqrMagnitude).ToArray();

        ScanPerformed?.Invoke(results);

        return results;
    }

    private IEnumerator PerformIntervaledScan()
    {
        var delay = new WaitForSeconds(_interval);
        
        while(_isTimed)
        {
            Scan(transform.position);
            yield return delay;
        }

        _scanCoroutine = null;
    }

    private void OnScanInputPerformed(InputAction.CallbackContext _)
    {
        Scan(transform.position);
    }
}