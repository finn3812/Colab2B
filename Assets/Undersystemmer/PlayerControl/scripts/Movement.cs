using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations; // If you're not using JetBrains Rider/ReSharper specific annotations, this might be removable.
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    // sebastian commit 
    // --- Inspector Fields ---
    [Header("Dependencies")]
    [SerializeField] StaminaSystem staminaSystem;
    [Header("Camera & Mouse Settings")]
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [Header("Movement Settings")]
    [SerializeField] float baseSpeed = 6.0f; // Used as the initial value for currentBaseSpeed
    [SerializeField] float sprintSpeedMultiplier = 1.6f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float staminaDrainPerSecond = 20f;
    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [Header("Headbob Settings")]
    [SerializeField] bool enableHeadbob = true;
    [SerializeField] float bobFrequency = 1.5f;
    [SerializeField] float bobAmplitude = 0.05f;
    [SerializeField] Transform headbobTarget;

    // --- Internal Runtime Variables ---
    private float currentBaseSpeed; // The *actual current* base speed (can be changed externally)
    private float currentSpeed;     // Speed right now (walk, sprint, or other modified speed)
    private CharacterController controller;
    private float velocityY;
    private bool isGrounded;

    // For height/center restoration
    private float originalHeight;
    private Vector3 originalCenter;

    // Camera/Mouse
    private float cameraCap;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    public bool IsSprint { get; private set; } = false; // Read-only from outside, set internally

    // Movement input
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;

    // Headbob
    private Vector3 initialHeadbobTargetLocalPos;
    private float bobTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!controller) Debug.LogError("FATAL: Movement script requires a CharacterController component!", this.gameObject);
        if (playerCamera == null) Debug.LogError("ERROR: playerCamera is not assigned!", this.gameObject);
        if (groundCheck == null) Debug.LogError("ERROR: groundCheck Transform is not assigned!", this.gameObject);
        if (staminaSystem == null) Debug.LogWarning("Movement: StaminaSystem is not assigned. Sprinting might not work as expected regarding stamina.", this.gameObject);


        currentBaseSpeed = baseSpeed; // Initialize with the Inspector value
        currentSpeed = currentBaseSpeed;

        originalHeight = controller.height;
        originalCenter = controller.center;
        Debug.Log($"Movement Start: Initial base speed={currentBaseSpeed:F1}, Original Height={originalHeight:F1}, Original Center={originalCenter}");

        if (headbobTarget == null && playerCamera != null) headbobTarget = playerCamera; // Default to playerCamera if not set
        if (headbobTarget) initialHeadbobTargetLocalPos = headbobTarget.localPosition;

        UpdateCursorState();
    }

    void Update()
    {
        if (!this.enabled)
        {
            // Reset movement input if script is disabled to prevent sliding
            currentDir = Vector2.zero;
            currentDirVelocity = Vector2.zero;
            return;
        }

        HandleGroundCheck();
        HandleMouseLook();
        HandleMovementInputAndSprint(); // Combined input and sprint logic
        HandleGravityAndJump();
        ApplyMovement(); // Single CharacterController.Move call
        if (enableHeadbob) ApplyHeadbob();
        HandleOtherKeybinds();
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleMouseLook()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
            cameraCap -= currentMouseDelta.y * mouseSensitivity;
            cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
            if (playerCamera) playerCamera.localEulerAngles = Vector3.right * cameraCap;
            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        }
    }

    void HandleMovementInputAndSprint()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        // Determine if player wants to sprint and can sprint
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        bool isMovingEnough = currentDir.magnitude > 0.1f;
        bool hasEnoughStamina = staminaSystem != null ? staminaSystem.HasStamina(staminaDrainPerSecond * Time.deltaTime) : true; // Check for enough stamina for this frame, or assume true if no stamina system

        if (wantsToSprint && isGrounded && isMovingEnough && hasEnoughStamina && currentBaseSpeed > 0.1f)
        {
            currentSpeed = currentBaseSpeed * sprintSpeedMultiplier;
            staminaSystem?.UseStamina(staminaDrainPerSecond * Time.deltaTime);
            IsSprint = true;
        }
        else
        {
            currentSpeed = currentBaseSpeed; // Use current base speed (could be walking or modified e.g. wheelchair)
            IsSprint = false;
        }
    }

    void HandleGravityAndJump()
    {
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; // A small negative value to keep the controller grounded
        }

        // Jump input - requires being grounded and not having a zero (or very low) base speed
        if (isGrounded && Input.GetButtonDown("Jump") && currentBaseSpeed > 0.1f)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocityY += gravity * Time.deltaTime; // Apply gravity continuously
    }

    void ApplyMovement()
    {
        if (controller == null) return;

        Vector3 horizontalVelocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed;
        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = velocityY; // Add vertical velocity (gravity/jump)

        controller.Move(finalVelocity * Time.deltaTime); // The single CharacterController.Move call per frame
    }

    void ApplyHeadbob()
    {
        if (!headbobTarget || controller == null) return;

        // Check if conditions for headbobbing are met
        bool canBob = isGrounded && controller.velocity.magnitude > 0.1f && currentSpeed > 0.1f && currentBaseSpeed > 0.1f;

        if (canBob)
        {
            // Calculate bob timer, scaling with speed ratio (sprint makes bob faster)
            // Ensure currentBaseSpeed is not zero to avoid division by zero error
            float speedRatio = (currentBaseSpeed > 0.01f) ? (currentSpeed / currentBaseSpeed) : 1.0f;
            bobTimer += Time.deltaTime * bobFrequency * speedRatio;

            float bobY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f; // Horizontal bob can be half frequency and amplitude
            Vector3 bobOffset = new Vector3(bobX, bobY, 0f);

            headbobTarget.localPosition = Vector3.Lerp(headbobTarget.localPosition, initialHeadbobTargetLocalPos + bobOffset, Time.deltaTime * 10f);
        }
        else
        {
            // Return to neutral position when not bobbing
            bobTimer = 0f; // Reset timer to avoid abrupt jump if bobbing resumes
            headbobTarget.localPosition = Vector3.Lerp(headbobTarget.localPosition, initialHeadbobTargetLocalPos, Time.deltaTime * 5f);
        }
    }

    void HandleOtherKeybinds()
    {
        // Example: 'E' might be handled by another script like SpinWheelController
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload (R) activated");
            // Add reload logic here if needed
        }
        // ... other keybinds ...
    }

    // -----===== PUBLIC METHODS FOR EXTERNAL CONTROL =====-----

    public void UpdateCursorState()
    {
        if (cursorLock && this.enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Sets the CURRENT base speed (e.g., for walking, or for wheelchair mode)
    public void SetSpeed(float newBaseSpeed)
    {
        currentBaseSpeed = Mathf.Max(0, newBaseSpeed); // Ensure speed is not negative
        // If not currently trying to sprint, update currentSpeed immediately.
        // If sprinting, currentSpeed will be updated in HandleMovementInputAndSprint.
        if (!IsSprint)
        {
            currentSpeed = currentBaseSpeed;
        }
        Debug.Log($"Movement - Base speed set to: {currentBaseSpeed:F1}");
    }

    public float GetBaseSpeed() { return currentBaseSpeed; }
    public float GetCurrentActualSpeed() { return currentSpeed; } // Speed including sprint/modifiers

    // --- NEW METHODS FOR HEIGHT/CENTER ADJUSTMENT ---

    public void SetCharacterHeightAndCenter(float newHeight)
    {
        if (controller == null) return;
        newHeight = Mathf.Max(0.1f, newHeight); // Prevent zero or negative height
        controller.height = newHeight;
        // Adjust center typically to half the height, maintaining original X/Z offset
        controller.center = new Vector3(originalCenter.x, newHeight / 2f, originalCenter.z);
        Debug.Log($"Movement - Height set to {controller.height:F1}, Center set to {controller.center}");
    }

    public void RestoreOriginalHeightAndCenter()
    {
        if (controller == null) return;
        controller.height = originalHeight;
        controller.center = originalCenter;
        Debug.Log($"Movement - Restored Original Height ({originalHeight:F1}) and Center ({originalCenter})");
    }

    public float GetOriginalHeight()
    {
        return originalHeight;
    }
}