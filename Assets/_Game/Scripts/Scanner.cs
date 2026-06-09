using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 20f;
    [SerializeField] private LayerMask _layersToScan;

    public Collider[] Scan(int limit, Vector3 position)
    {
        Collider[] results = new Collider[limit];

        Physics.OverlapSphereNonAlloc(position, _maxRadius, results, _layersToScan);

        results = results.Where(collider => collider != null).OrderBy(collider => (collider.transform.position - position).sqrMagnitude).ToArray();

        return results;
    }
}