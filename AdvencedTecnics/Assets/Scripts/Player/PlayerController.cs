using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _movementSpeed;

    [Header("References")]
    Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        MoveInDirection(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    private void MoveInDirection(Vector2 direction)
    {
        Vector3 finalVelocity = (direction.x * transform.right + direction.y * transform.forward).normalized * _movementSpeed;

        finalVelocity.y = _rigidBody.linearVelocity.y;
        _rigidBody.linearVelocity = finalVelocity;
    }
}
