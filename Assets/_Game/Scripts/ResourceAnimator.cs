using DG.Tweening;
using UnityEngine;

public class ResourceAnimator
{
    private Transform _transform;
    private PickableAnimationConfig _config;

    public ResourceAnimator(Transform transform, PickableAnimationConfig config)
    {
        _config = config;
        _transform = transform;
    }

    public Tween PlayPickUp(Vector3 endPosition)
    {
        return DoArcMove(endPosition, _config.PickUpHeight, _config.PickUpDuration);
    }

    public Tween PlayDrop(Vector3 endPosition)
    {
        return DoArcMove(endPosition, _config.DropHeight, _config.DropDuration);
    }

    private Tween DoArcMove(Vector3 endPosition, float curveHeight, float duration)
    {
        Vector3 startPosition = _transform.position;

        Vector3 middlePosition = (startPosition + endPosition) * MathConstants.Half;
        middlePosition = middlePosition.Change(y: middlePosition.y + curveHeight);

        Vector3[] path = new Vector3[] {startPosition, middlePosition, endPosition};

        return _transform.DOPath(path, duration, PathType.CatmullRom);
    }
}