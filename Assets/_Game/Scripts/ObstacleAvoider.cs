using UnityEngine;

public class ObstacleAvoider : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacles;

    public bool TryGetWorkaround(Vector3 targetPosition, out Vector3 direction)
    {
        bool obstacleFound;
        float castDistance = 10f;
        Vector3 obstacleCheckerBounds = Vector3.one * MathConstants.Half;
        Vector3 targetDirection = transform.position.DirectionTo(targetPosition);

        obstacleFound = Physics.BoxCast(transform.position, obstacleCheckerBounds, targetDirection, 
            out RaycastHit hit, Quaternion.identity, castDistance, _obstacles);

        if (obstacleFound && IsObstacleCloserThanTarget(hit.point, targetPosition))
        {
            direction = GetWorkaroundDirection(targetDirection, hit.normal).Change(y: 0);

            return true;
        }
        else
        {
            direction = targetDirection.Change(y: 0);

            return false;
        }
    }

    private bool IsObstacleCloserThanTarget(Vector3 obstaclePosition, Vector3 targetPosition)
    {
        return obstaclePosition.SqrDistanceTo(transform.position) < targetPosition.SqrDistanceTo(transform.position);
    }

    private Vector3 GetWorkaroundDirection(Vector3 targetDirection, Vector3 obstacleNormal)
    {
        float avoidJitteringThreshold = 0.2f;

        Vector3 obstacleTangent = Vector3.Cross(obstacleNormal, Vector3.up).normalized;

        float DOT = Vector3.Dot(targetDirection, obstacleTangent);

        return Mathf.Abs(DOT) < avoidJitteringThreshold ? obstacleTangent : Mathf.Sign(DOT) * obstacleTangent;
    }
}