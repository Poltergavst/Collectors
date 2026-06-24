using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour, IPickable
{
    [SerializeField] private PickableAnimationConfig _config;

    private Collider _collider;
    private ResourceSpawner _spawner;
    private ResourceAnimator _animator;

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        _animator = new(transform, _config);
        _collider = GetComponent<Collider>();
    }

    public Vector3 GetCoordinates()
    {
        return transform.position;
    }

    public YieldInstruction PickUp(Transform container)
    {
        Tween tween = _animator.PlayPickUp(container.position).OnComplete(() => transform.SetParent(container));

        return tween.WaitForCompletion();
    }

    public void Drop(Vector3 dropPosition)
    {
        _animator.PlayDrop(dropPosition).OnComplete(() =>
        {
            transform.SetParent(_spawner.gameObject.transform);
            _spawner.Release(this);
        });
    }

    public void AssignSpawner(ResourceSpawner spawner)
    {
        _spawner = spawner;
    }

    public void DisableForDetection()
    {
        _collider.enabled = false;
    }

    public void EnableForDetection()
    {
        _collider.enabled = true;
    }
}
