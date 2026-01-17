using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputActionMap playerActionMap;

    public static Vector2 moveDirection;
    public static bool isCrouchHeld;
    public static bool isJumpPressed;
    public static bool isJumpHeld;
    public static bool isJumpReleased;
    public static bool isThrowPressed;
    public static bool isUsePressed;

    private InputAction moveAction, crouchAction, jumpAction, throwAction, useAction;

    private void Awake()
    {
        playerActionMap = InputSystem.actions.FindActionMap("Player"); //Store AM ref if we need to disable/enable later
        moveAction = playerActionMap.FindAction("Move");
        jumpAction = playerActionMap.FindAction("Jump");
        crouchAction = playerActionMap.FindAction("Crouch");
        throwAction = playerActionMap.FindAction("Throw");
        useAction = playerActionMap.FindAction("Use");
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        isCrouchHeld = crouchAction.IsPressed();

        isJumpPressed = jumpAction.WasPressedThisFrame();
        isJumpHeld = jumpAction.IsPressed();
        isJumpReleased = jumpAction.WasReleasedThisFrame();

        isThrowPressed = throwAction.WasPressedThisFrame();
        isUsePressed = useAction.WasPressedThisFrame();
    }
}
