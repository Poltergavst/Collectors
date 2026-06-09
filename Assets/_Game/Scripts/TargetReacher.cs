using System.Collections;
using UnityEngine;

public class TargetReacher : MonoBehaviour
{
    private UnitMover _unitMover;

    public void Initialize(UnitMover unitMover)
    {
        _unitMover = unitMover;
    }

    public IEnumerator ReachTarget(Vector3 target, float stoppingDistance)
    {
        Vector3 direction = transform.position.DirectionTo(target);

        while (transform.position.IsEnoughCloseTo(target, stoppingDistance) == false)
        {
            _unitMover.Move(direction);
            _unitMover.Rotate(direction);

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
            _unitMover.Rotate(direction);
            yield return null;
        }
    }
}
