using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(ObstacleAvoider), typeof(TargetReacher))]
[RequireComponent(typeof(Carrier))]
public class Unit : MonoBehaviour
{
    private Carrier _carrier;
    private UnitMover _unitMover;
    private TargetReacher _reacher;
    private ObstacleAvoider _obstacleAvoider;

    private void Awake()
    {
        _carrier = GetComponent<Carrier>();
        _unitMover = GetComponent<UnitMover>();
        _reacher = GetComponent<TargetReacher>();
        _obstacleAvoider = GetComponent<ObstacleAvoider>();

        _reacher.Initialize(_unitMover);
    }

    public IEnumerator NavigateTo(Vector3 target)
    {
        Vector3 direction;
        float stoppingDistance = 1.25f;

        while (_obstacleAvoider.TryGetWorkaround(target, out direction))
        {
            _unitMover.Move(direction);
            _unitMover.RotateTo(direction);

            yield return null;
        }

        yield return _reacher.ReachTarget(target, stoppingDistance);
    }

    public IEnumerator ReturnToBase(Base homeBase)
    {
        float offset = 1f;
        float baseRadius = homeBase.GetComponent<Collider>().bounds.extents.x + offset;
        Vector3 basePosition = homeBase.transform.position;

        yield return _reacher.ReachTarget(basePosition, baseRadius);
    }

    public IEnumerator Collect(IPickable pickable, Base homeBase)
    {
        yield return NavigateTo(pickable.GetCoordinates());
        yield return _carrier.PickUp(pickable);
        yield return ReturnToBase(homeBase);
        
        _carrier.Drop(homeBase.transform.position);
    }
}