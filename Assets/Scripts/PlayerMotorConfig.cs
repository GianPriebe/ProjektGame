using UnityEngine;

public class PlayerMotorConfig : MonoBehaviour
{
    [Header ("Movement")]
    public float moveSpeed = 0f;
    public float walkSpeed = 8f;
    public float runSpeed = 12f;
    public float moveMultiplier = 10f;
    public bool canRun = true;
    public float StaminaPoints = 10f;
    public float lastMoveInt = 0f;

    [Header ("Crouch")]
    public bool isCrouched;
    public float heightCrouched = 1f;

    [Header ("Jump")]
    public float jumpForce = 7f;
    public float groundDrag = 6f;
    public float airDrag = 2f;
    public float dragMultiplier = 0.217f;

    [Header ("Ground Check")]
    public float groundCheckRadiusBuffer = 0.05f;
    public float groundCheckBuffer = 0.1f;
    public LayerMask groundLayerMask = ~0;
    public float groundCheckDistance = 0.1f;

    [Header ("Character")]
    [SerializeField] public float height;
    [SerializeField] public float radius;

    [Header("Headbob")]
    public bool headbob_Enable = true;
    public float headbob_MinSpeedToBob = 0.1f;
    public float headBobProgress = 0f;
    public AnimationCurve headbob_VerticalAnimationCurve;
    public AnimationCurve headbob_StopAnimationCurve;

    [Header ("Camera")]
    public float camera_VerticalOffset = -0.1f;
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float keyFrameRightAnimationCurve = 0f;
    public float keyFrameLeftAnimationCurve = 0f;
    public float lastHorizontalState = 0f;
    public AnimationCurve sidesAnimationCurve;

    private void Start() {
        Transform playerBody = transform.Find("PlayerBody");
        CapsuleCollider collider = playerBody.gameObject.GetComponent<CapsuleCollider>();
        height = collider.height;
        radius = collider.radius;
    }
}
