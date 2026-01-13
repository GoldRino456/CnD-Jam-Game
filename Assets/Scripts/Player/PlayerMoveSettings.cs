using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMoveSettings", menuName = "Player/PlayerMoveSettings")]
public class PlayerMoveSettings : ScriptableObject
{
    [Header("Movement")]
    public float maxMoveSpeed = 15f;
    public float groundAcceleration = 5f;
    public float groundDeceleration = 20f;
    public float airAcceleration = 5f;
    public float airDeceleration = 5f;

    [Header("Collision Checks")]
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.02f;
    public float headDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float headWidth = 0.75f;

    [Header("Jump")]
    public float jumpHeight = 7f;
    [Range(1f, 1.1f)] public float jumpHeightCompensationFactor = 1.054f;
    public float timeUntilJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    public float maxFallSpeed = 26f;
    [Range(1, 5)] public int numJumpsAllowed = 2;
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;
    [Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float apexHangTime = 0.075f;
    [Range(0f, 1f)] public float jumpBufferTime = 0.125f;
    [Range(0f, 1f)] public float jumpCoyoteTime = 0.1f;

    [Header("Jump Visualization")]
    public bool showJumpArc = false;
    public bool stopOnCollision = true;
    public bool drawRight = true;
    [Range(5, 100)] public int arcResolution = 20;
    [Range(0, 500)] public int visualizationSteps = 90;

    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeUntilJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * timeUntilJumpApex;
    }
}
