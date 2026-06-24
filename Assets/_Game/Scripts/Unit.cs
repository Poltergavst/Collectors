using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(ObstacleAvoider), typeof(TargetReacher))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _hand; 

    private UnitMover _unitMover;
    private ObstacleAvoider _obstacleAvoider;
    private TargetReacher _reacher;

    public Transform Hand => _hand;

    private void Awake()
    {
        _unitMover = GetComponent<UnitMover>();
        _obstacleAvoider = GetComponent<ObstacleAvoider>();
        _reacher = GetComponent<TargetReacher>();

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
}