using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private InputActionReference throwActionRef; //Throw Item
    [SerializeField] private InputActionReference useActionRef; //Use Item or Pick Up

    public static Vector2 moveDirection;
    public static bool isJumpPressed;
    public static bool isJumpHeld;
    public static bool isJumpReleased;

    private InputAction moveAction, jumpAction;

    private void Awake()
    {
        moveAction = moveActionRef.action;
        jumpAction = jumpActionRef.action;
    }

    private void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        isJumpPressed = jumpAction.WasPressedThisFrame();
        isJumpHeld = jumpAction.IsPressed();
        isJumpReleased = jumpAction.WasReleasedThisFrame();
    }
}
