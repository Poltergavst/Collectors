using UnityEngine;

public class Mover: MonoBehaviour
{
    [SerializeField, SerializeInterface(typeof(IMoveable))] private GameObject _movement;

    private IMoveable _movable;

    private void Start()
    {
        _movable = _movement.GetComponent<IMoveable>();
        _movable.Move();    
    }
}
