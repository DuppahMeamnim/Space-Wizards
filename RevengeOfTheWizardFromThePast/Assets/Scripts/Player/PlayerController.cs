using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 7.0f;
    //sprint??

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = inputHandler.MoveInput;

        rb.linearVelocity = input.normalized * walkSpeed;
        //linear velocity for now because force will push the hell out of the player.
    }
}
