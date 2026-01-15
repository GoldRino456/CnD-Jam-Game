using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private InputActionReference crouchActionRef;
    [SerializeField] private InputActionReference throwActionRef; //Throw Item
    [SerializeField] private InputActionReference useActionRef; //Use Item or Pick Up

    public static Vector2 moveDirection;
    public static bool isCrouchHeld;
    public static bool isJumpPressed;
    public static bool isJumpHeld;
    public static bool isJumpReleased;

    private InputAction moveAction, crouchAction, jumpAction;

    private void Awake()
    {
        moveAction = moveActionRef.action;
        crouchAction = crouchActionRef.action;
        jumpAction = jumpActionRef.action;
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        isCrouchHeld = crouchAction.IsPressed();

        isJumpPressed = jumpAction.WasPressedThisFrame();
        isJumpHeld = jumpAction.IsPressed();
        isJumpReleased = jumpAction.WasReleasedThisFrame();
    }
}
