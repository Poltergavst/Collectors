using System.Collections;
using UnityEngine;

public class TargetReacher : MonoBehaviour
{
    private UnitMover _unitMover;
    private ObstacleAvoider _obstacleAvoider;

    public void Initialize(UnitMover unitMover, ObstacleAvoider obstacleAvoider)
    {
        _unitMover = unitMover;
        _obstacleAvoider = obstacleAvoider;
    }

    public IEnumerator ReachTarget(Transform target, float stoppingDistance)
    {
        Vector3 direction = transform.position.DirectionTo(target.position).Change(y:0);

        while (transform.position.IsEnoughCloseTo(target.position, stoppingDistance) == false)
        {
            _obstacleAvoider.TryGetWorkaround(target, out direction);

            _unitMover.Move(direction);
            _unitMover.RotateTo(direction);

            yield return null;
        }

        yield return FinishRotation(direction);
    }

    private IEnumerator FinishRotation(Vector3 direction)
    {
        float threshold = 0.2f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > threshold)
        {
            _unitMover.RotateTo(direction);
            yield return null;
        }
    }
}
