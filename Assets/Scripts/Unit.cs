using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _hand; 

    private Transform _base;
    public event Action<Unit, IPickable> Delivered;

    public bool IsDelivering { get; set; }

    public void StartCollection(IPickable pickable)
    {
        StartCoroutine(CollectResource(pickable));
    }

    public void SetBase(Transform homeBase)
    {
        _base = homeBase;
    }

    private IEnumerator ReachTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        direction = new Vector3(direction.x, 0f, direction.z).normalized;
        //transform.LookAt(target);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while ((target - transform.position).sqrMagnitude > 0.2f)
        {
            transform.Translate(_speed * Time.deltaTime * direction, Space.World);

            if (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _speed);
            }

            yield return null;
        }
    }

    private void PickUpTarget(IPickable pickable)
    {
        pickable.PickUp(_hand);
    }

    private IEnumerator CollectResource(IPickable pickable)
    {
        IsDelivering = true;
        yield return ReachTarget(pickable.GetCoordinates());
        PickUpTarget(pickable);
        yield return ReachTarget(_base.position);

        Delivered?.Invoke(this, pickable);
        IsDelivering = false;
        Debug.Log("Delivered");
    }
}