using System;
using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private int _limit = 10;
    [SerializeField] private float _radius = 20f;
    [SerializeField] private int _interval = 5;
    [SerializeField] private LayerMask _layersToScan;

    private Collider[] _results;
    private Coroutine _scanCoroutine;
    private ColliderDistanceComparer _comparer;

    public event Action<Collider[]> ScanPerformed;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _interval = Mathf.Max(0, _interval);

        if (Application.isPlaying == false)
            return;

        if (_scanCoroutine == null && _results != null)
        {
            _scanCoroutine = StartCoroutine(PerformIntervaledScan());
        }
    }
#endif

    private void Start()
    {
        _results = new Collider[_limit];
        _comparer = new ColliderDistanceComparer();
        _scanCoroutine = StartCoroutine(PerformIntervaledScan());
    }

    private Collider[] Scan(Vector3 position)
    {
        int count = Physics.OverlapSphereNonAlloc(position, _radius, _results, _layersToScan);

        _comparer.SetPoistion(position);
        Array.Sort(_results, 0, count, _comparer);

        ScanPerformed?.Invoke(_results);

        return _results;
    }

    private IEnumerator PerformIntervaledScan()
    {
        var delay = new WaitForSeconds(_interval);

        while (enabled)
        {
            Scan(transform.position);
            yield return delay;
        }

        _scanCoroutine = null;
    }
}