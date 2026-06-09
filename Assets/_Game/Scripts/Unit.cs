using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(ObstacleAvoider), typeof(TargetReacher))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _hand; 

    private Base _base;
    private UnitMover _unitMover;
    private ObstacleAvoider _obstacleAvoider;
    private TargetReacher _reacher;

    public event Action<Unit, IPickable> Delivered;

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        _obstacleAvoider = GetComponent<ObstacleAvoider>();
        _reacher = GetComponent<TargetReacher>();

        _reacher.Initialize(_unitMover);
    }

    public void SetBase(Base homeBase)
    {
        _base = homeBase;
    }

    public void StartCollection(IPickable pickable)
    {
        StartCoroutine(CollectResource(pickable));
    }

    private IEnumerator CollectResource(IPickable pickable)
    {
        yield return GoForPickable(pickable.GetCoordinates());
        yield return pickable.PickUp(_hand);
        yield return DeliverToBase();

        Delivered?.Invoke(this, pickable);
    }

    private IEnumerator GoForPickable(Vector3 target)
    {
        Vector3 direction;
        float stoppingDistance = 0.2f;

        while (_obstacleAvoider.TryGetWorkaround(target, out direction))
        {
            _unitMover.Move(direction);
            _unitMover.Rotate(direction);

            yield return null;
        }

        yield return _reacher.ReachTarget(target, stoppingDistance);
    }

    private IEnumerator DeliverToBase()
    {
        float baseRadius = _base.GetComponent<Collider>().bounds.extents.x + 1;

        yield return _reacher.ReachTarget(_base.transform.position, baseRadius);
    }
}