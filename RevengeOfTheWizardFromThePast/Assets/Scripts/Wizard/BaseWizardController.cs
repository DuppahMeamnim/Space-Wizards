using UnityEngine;

public class BaseWizardController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 20.0f;
    [SerializeField] private float acceleration = 4000.0f;
    [SerializeField] private float deceleration = 10000.0f;
    [SerializeField] private string currentState;

    //Animation States
    const string WIZARD_IDLE = "Wizard_idle";
    const string WIZARD_RUN = "Wizard_run";

    private Rigidbody2D rb;
    private Animator animator;
    private WizardInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<WizardInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        if(input.magnitude > 0)
        {
            ChangeAnimationState(WIZARD_RUN);
        }
        else
        {
            ChangeAnimationState(WIZARD_IDLE);
        }

        Vector3 originalScale = transform.localScale;

        if (input.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}

