using UnityEngine;

public class ObstacleAvoider : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacles;

    public bool TryGetWorkaround(Transform target, out Vector3 direction)
    {
        bool obstacleFound;
        float castDistance = 10f;
        Vector3 obstacleCheckerBounds = Vector3.one * MathConstants.Half;
        Vector3 targetPosition = target.position;
        Vector3 targetDirection = transform.position.DirectionTo(targetPosition).Change(y: 0);

        obstacleFound = Physics.BoxCast(transform.position, obstacleCheckerBounds, targetDirection, 
            out RaycastHit hit, Quaternion.identity, castDistance, _obstacles) 
            && IsHomeBaseAhead(hit, target) == false
            && IsObstacleCloserThanTarget(hit.point, targetPosition);

        if (obstacleFound)
        {
            direction = GetWorkaroundDirection(targetDirection, hit.normal);
            return true;
        }

        direction = targetDirection;

        return false;
    }

    private bool IsObstacleCloserThanTarget(Vector3 obstaclePosition, Vector3 targetPosition)
    {
        return obstaclePosition.SqrDistanceTo(transform.position) < targetPosition.SqrDistanceTo(transform.position);
    }

    private bool IsHomeBaseAhead(RaycastHit hit, Transform target)
    {
        return hit.transform == target;
    }

    private Vector3 GetWorkaroundDirection(Vector3 targetDirection, Vector3 obstacleNormal)
    {
        float avoidJitteringThreshold = 0.2f;

        Vector3 tangentToObstacle = Vector3.Cross(obstacleNormal, Vector3.up).normalized;

        float DOT = Vector3.Dot(targetDirection, tangentToObstacle);

        return Mathf.Abs(DOT) < avoidJitteringThreshold ? tangentToObstacle : Mathf.Sign(DOT) * tangentToObstacle;
    }
}