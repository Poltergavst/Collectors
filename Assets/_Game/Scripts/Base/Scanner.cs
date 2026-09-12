using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private int _limit = 10;
    [SerializeField] private float _radius = 20f;
    [SerializeField] private int _interval = 5;
    [SerializeField] private LayerMask _layersToScan;

    private Coroutine _scanCoroutine;

    public event Action<Collider[]> ScanPerformed;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _interval = Mathf.Max(0, _interval);

        if (Application.isPlaying == false)
            return;

        if (_scanCoroutine == null)
        {
            _scanCoroutine = StartCoroutine(PerformIntervaledScan());
        }
    }
#endif

    private void Start() => _scanCoroutine = StartCoroutine(PerformIntervaledScan());

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
        
        while(enabled)
        {
            Scan(transform.position);
            yield return delay;
        }

        _scanCoroutine = null;
    }
}