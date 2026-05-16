using UnityEngine;

public class ForwardMovement : MonoBehaviour, IMoveable
{
    [SerializeField] private float _step = 10f;

    public void Move()
    {
        transform.Translate(Vector3.forward * _step);
        Debug.Log("I fuckn moved");
    }
}

