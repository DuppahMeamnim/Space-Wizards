using UnityEngine;
using UnityEngine.InputSystem;

public class WizardInputHandler : MonoBehaviour
{

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset wizardControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Wizard";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";

    private InputAction moveAction;

    public Vector2 MoveInput { get; private set; }

    public static WizardInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = wizardControls.FindActionMap(actionMapName).FindAction(move);

        RegisterInputActions();
    }
    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        moveAction.Enable();  
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }
}
