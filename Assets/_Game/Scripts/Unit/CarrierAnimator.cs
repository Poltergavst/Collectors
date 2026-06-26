using DG.Tweening;
using UnityEngine;

public class CarrierAnimator
{
    private CarrierAnimationsConfig _config;

    public CarrierAnimator(CarrierAnimationsConfig config) => _config = config;

    public Tween PlayPickUp(Transform transform, Vector3 endPosition) =>
        DoArcMove(transform, endPosition, _config.PickUpHeight, _config.PickUpDuration);

    public Tween PlayDrop(Transform transform, Vector3 endPosition) => 
        DoArcMove(transform, endPosition, _config.DropHeight, _config.DropDuration);

    private Tween DoArcMove(Transform transform, Vector3 endPosition, float curveHeight, float duration)
    {
        Vector3 startPosition = transform.position;

        Vector3 middlePosition = (startPosition + endPosition) * MathConstants.Half;
        middlePosition = middlePosition.Change(y: middlePosition.y + curveHeight);

        Vector3[] path = new Vector3[] {startPosition, middlePosition, endPosition};

        return transform.DOPath(path, duration, PathType.CatmullRom);
    }
}