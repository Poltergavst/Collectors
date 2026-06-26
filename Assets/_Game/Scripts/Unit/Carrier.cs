using DG.Tweening;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [SerializeField] private PickableAnimationConfig _config;
    [SerializeField] private Transform _hand;

    private IPickable _pickable;
    private PickableAnimator _animator;

    private void Awake()
    {
        _animator = new(_config);
    }

    public YieldInstruction PickUp(IPickable pickable)
    {
        _pickable = pickable;

        Tween tween = _animator.PlayPickUp(_pickable.GameObject.transform, _hand.position).OnComplete(() => 
            pickable.GameObject.transform.SetParent(_hand));

        return tween.WaitForCompletion();
    }

    public void Drop(Vector3 dropPosition)
    {
        _animator.PlayDrop(_pickable.GameObject.transform, dropPosition).OnComplete(() =>
        {
            if (_pickable == null)
                return;

            _pickable.GameObject.transform.SetParent(null);
            _pickable.Despawn();
        });
    }
}