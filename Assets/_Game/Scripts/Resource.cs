using DG.Tweening;
using UnityEngine;
using static UnityEditor.Progress;

public class Resource : MonoBehaviour, IPickable
{
    public GameObject GameObject => gameObject;

    private ResourceSpawner _spawner;

    public Vector3 GetCoordinates()
    {
        return transform.position;
    }

    public YieldInstruction PickUp(Transform container)
    {
        Vector3 position = transform.position;
        Vector3 containerPosition = container.position;

        Vector3 middlePoint = transform.position + containerPosition / 2f;

        transform.SetParent(container);

        Tween tween = transform.DOLocalPath(new Vector3[] { transform.localPosition, transform.localPosition + new Vector3(0, 3, 0) ,Vector3.zero }, 0.5f, PathType.CatmullRom);

        return tween.WaitForCompletion();
    }

    public void Drop()
    {
        Tween tween = transform.DOPath(new Vector3[] { transform.position, transform.position / 2f + new Vector3(0, 6, 0), Vector3.zero }, 0.6f, PathType.CatmullRom).SetEase(Ease.OutSine);

        tween.OnComplete(() =>
        {
            transform.SetParent(_spawner.gameObject.transform);
            _spawner.Release(this);
        });
    }

    public void AssignSpawner(ResourceSpawner spawner)
    {
        _spawner = spawner;
    }
}
