using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 20f;
    [SerializeField] private LayerMask _layersToScan;

    public Collider[] Scan(int limit)
    {
        Collider[] results = new Collider[limit];

        Physics.OverlapSphereNonAlloc(transform.position, _maxRadius, results, _layersToScan);

        return results;
    }
}

//    private IEnumerator ShowScan(int limit)
//    {
//        float radius = 0f;
//        float speed = 2f;

//        Collider[] scanned = Scan(limit);
//        Collider[] sorted = null;
//        float[] magnitudes = null;

//        if (scanned.Length > 0)
//        {
//            sorted = scanned.Where(collider => collider != null).OrderBy(collider => (collider.transform.position - transform.position).sqrMagnitude).ToArray();
//            magnitudes = sorted.Select(collider => (collider.transform.position - transform.position).magnitude).ToArray();   
//        }

//        float breakpoint = _maxRadius;
//        int index = 0;
//        int maxIndex = magnitudes.Length;

//        Debug.Log(magnitudes.Length);

//        if (magnitudes != null)
//        {
//            breakpoint = magnitudes[index];
//        }

//        Debug.Log("Hewwow");

//        while (radius < breakpoint)
//        {
//            radius += Time.deltaTime * speed;

//            Debug.Log($"{radius} - {breakpoint}");

//            if(radius >= breakpoint)
//            {
//                //MaterialPropertyBlock block = new ();
//                //block.SetColor("_BaseColor", Color.red);
//                //sorted[index].gameObject.GetComponent<Renderer>().SetPropertyBlock(block);

//                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
//                materialPropertyBlock.SetColor("_BaseColor", Color.red);

//                sorted[index].gameObject.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);

//                Debug.Log(sorted[index].name);

//                if (index <= maxIndex)
//                {
//                    breakpoint = magnitudes[index++];
//                }

//                breakpoint = _maxRadius;
//            }

//            yield return null;
//        }
//    }
//}
