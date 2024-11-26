using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 20.0f;
    [SerializeField] private float acceleration = 4000.0f;
    [SerializeField] private float deceleration = 10000.0f;

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = inputHandler.MoveInput;
        Vector2 targetVelocity = input * walkSpeed;

        Vector2 velocityChange = targetVelocity - rb.linearVelocity;
        float rate = input.magnitude > 0 ? acceleration : deceleration;
        Vector2 movementForce = velocityChange * rate * Time.fixedDeltaTime;

        rb.AddForce(movementForce, ForceMode2D.Force);
    }
}
