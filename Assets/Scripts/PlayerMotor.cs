using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class PlayerMotor : MonoBehaviour
{
    [Header ("HeadBob")]
    protected float headbobProgress = 0f;
    public GameObject body;
    public bool isGrounded = true;
    Rigidbody rigidbodyPlayer;
    PlayerMotorConfig config;
    public Vector3 gravityDirection = new Vector3(0f, -9.81f, 0f);
    Camera mainCamera;
    public Transform car;
    public bool inSitting = false;
    public CapsuleCollider colliderPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
        rigidbodyPlayer.freezeRotation = true;
        config = GetComponent<PlayerMotorConfig>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        ControlDrag();
        GetKeyCrouch();
    }

    private void FixedUpdate() 
    {
        //RaycastHit groundCheckResult = UpdateIsGrounded();
        //rb.AddForce(gravityDirection, ForceMode.Acceleration);
        if (!inSitting)
        {
            MovePlayer();
        }
        else
        {
            float horizontalMovement = Input.GetAxisRaw("Horizontal");

            if (horizontalMovement != 0f)
            {
                config.lastMoveInt = horizontalMovement;
            } 
            else
            {
                config.lastMoveInt = 0f;
            }
        }
        Crouch();
    }

    private void LateUpdate() {
        UpdateCamera();
    }

    void MovePlayer()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            rigidbodyPlayer.AddForce(new Vector3(0f, 0f, 0f));
            rigidbodyPlayer.AddForce(new Vector3(0, config.jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            config.isCrouched = false;
        }
        //Debug.Log(config.StaminaPoints);
        if (Input.GetKey(KeyCode.LeftShift) && config.StaminaPoints > 0f)
        {
            config.moveSpeed = config.runSpeed;
            config.StaminaPoints -= .1f;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && config.StaminaPoints < 10f)
        {
            config.moveSpeed = config.walkSpeed;
            config.StaminaPoints += .5f;
        }

        config.StaminaPoints = Mathf.Clamp(config.StaminaPoints, 0, 10);
    
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.forward * verticalMovement + transform.right * horizontalMovement;

        if (isGrounded)
        {
            rigidbodyPlayer.AddForce(movement.normalized * config.moveSpeed * config.moveMultiplier , ForceMode.Acceleration);
        }
        else
        {
            rigidbodyPlayer.AddForce(movement.normalized * config.moveSpeed * config.moveMultiplier * config.dragMultiplier, ForceMode.Acceleration);
        }//Mathf.Sqrt (jumpHeight * -2f * Physics.gravity.y)
    }

    private void UpdateCamera() {
        // Captura a entrada do mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * config.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * config.mouseSensitivity * Time.deltaTime;

        // Ajusta a rotação no eixo X (para olhar para cima e para baixo)
        config.yRotation += mouseX;
        config.xRotation -= mouseY;
        config.xRotation = Mathf.Clamp(config.xRotation, -90f, 90f);

        // Aplica a rotação da câmera
        Quaternion mainCamRot = mainCamera.transform.localRotation;
        //mainCamera.transform.localRotation = Quaternion.Euler(config.xRotation, mainCamRot.y, mainCamRot.z);
        transform.rotation = Quaternion.Euler(0f, config.yRotation, 0f);
        Vector3 camPos = new Vector3(transform.position.x, transform.position.y+(transform.localScale.y*1.5f), transform.position.z);
        mainCamera.transform.position = camPos;

        CameraVRotateHeadBob();
        CameraZRotateWhenMoveHorizontal();
    }

    void CameraVRotateHeadBob()
    {
        float playerVelocity = rigidbodyPlayer.linearVelocity.magnitude;
        if (playerVelocity > 0.1f)
        {
            if (config.headBobProgress >= 1)
            {
                config.headBobProgress = 0f;
            }
            config.headBobProgress += .001f * config.moveSpeed;
        }
        else if (config.headBobProgress < 1f && config.headBobProgress != 0f)
        {
            if (playerVelocity >= 1f)
            {
                config.headBobProgress = 0f;
            }
            config.headBobProgress += .025f;
        }

        //config.headBobProgress = Mathf.Clamp(config.headBobProgress, 0, 1);
        float verticalValue = config.headbob_VerticalAnimationCurve.Evaluate(config.headBobProgress)*(config.moveSpeed*1.1f);
        Quaternion mainCamRot = mainCamera.transform.rotation;
        //Debug.Log(verticalValue);
        float zRotation = CameraZRotateWhenMoveHorizontal();
        mainCamera.transform.rotation = Quaternion.Euler(config.xRotation-verticalValue, config.yRotation, zRotation);
    }
    private float CameraZRotateWhenMoveHorizontal()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 && config.keyFrameRightAnimationCurve <= 1f)
        {
            config.lastHorizontalState = 1;
            config.keyFrameRightAnimationCurve += .025f;
            if (config.keyFrameLeftAnimationCurve > 0f)
            {
                config.keyFrameLeftAnimationCurve -= 0.025f;
            }
            else
            {
                config.keyFrameLeftAnimationCurve = 0f;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && config.keyFrameLeftAnimationCurve <= 1f)
        {
            config.lastHorizontalState = 1;
            config.keyFrameLeftAnimationCurve += .025f;
            if (config.keyFrameRightAnimationCurve > 0f)
            {
                config.keyFrameRightAnimationCurve -= 0.025f;
            }
            else
            {
                config.keyFrameRightAnimationCurve = 0f;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (config.keyFrameRightAnimationCurve > 0f)
            {
                config.keyFrameRightAnimationCurve -= .05f;
            }
            else
            {
                config.keyFrameRightAnimationCurve = 0;
            }

            if (config.keyFrameLeftAnimationCurve > 0f)
            {
                config.keyFrameLeftAnimationCurve -= .05f;
            }
            else
            {
                config.keyFrameLeftAnimationCurve = 0;
            }
        }
        Quaternion mainCamRot = mainCamera.transform.rotation;
        float zRotation = mainCamRot.z+config.sidesAnimationCurve.Evaluate(config.keyFrameLeftAnimationCurve)-config.sidesAnimationCurve.Evaluate(config.keyFrameRightAnimationCurve);
        return zRotation;
    }

    private void GetKeyCrouch()
    {
        bool crouchThisFrame = false;
        if (Input.GetKeyDown(KeyCode.LeftControl) && config.isCrouched == false)
        {
            config.isCrouched = true;
            crouchThisFrame = true;
        } else if (Input.GetKeyDown(KeyCode.LeftControl) && config.isCrouched == true && crouchThisFrame == false)
        {
            config.isCrouched = false;
        }
    }
    private void Crouch()
    {
        if (config.isCrouched == true && transform.localScale.y >= config.heightCrouched)
        {
            float newHeight = Mathf.Lerp(transform.localScale.y, config.heightCrouched, .1f);
            transform.localScale = new Vector3(1, newHeight, 1);
        }
        else if (config.isCrouched == false && transform.localScale.y < 1f)
        {
            float newHeight = Mathf.Lerp(transform.localScale.y, 1f, .1f);
            transform.localScale = new Vector3(1, newHeight, 1);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rigidbodyPlayer.linearDamping = config.groundDrag;
        } 
        else
        {
            rigidbodyPlayer.linearDamping = config.airDrag;
        }
    }
    
    protected void UpdateIsGrounded(Collision collision)
    {
        Vector3 startPos = new Vector3(config.transform.position.x, config.transform.position.y-(config.height*.26f), config.transform.position.z);
        
        //RaycastHit hit;
        Collider[] hitColliders = Physics.OverlapSphere(startPos, config.radius, config.groundLayerMask, QueryTriggerInteraction.Ignore);
        if (hitColliders.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //Debug.Log(hitColliders.Length);
        //return hit;
    }

    private void OnCollisionEnter(Collision other) {
        UpdateIsGrounded(other);
    }
    private void OnDrawGizmos() {
        config = GetComponent<PlayerMotorConfig>();
        Gizmos.color = Color.red;
        Vector3 startPos = new Vector3(config.transform.position.x, config.transform.position.y-(config.height*.2f), config.transform.position.z);
        Gizmos.DrawSphere(startPos, config.radius + config.groundCheckRadiusBuffer);
    }
}