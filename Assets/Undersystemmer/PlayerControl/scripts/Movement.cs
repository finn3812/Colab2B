using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    // --- Inspector Fields --- (Uændret sektion)
    [Header("Dependencies")]
    [SerializeField] StaminaSystem staminaSystem;
    [Header("Camera & Mouse Settings")]
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [Header("Movement Settings")]
    [SerializeField] float baseSpeed = 6.0f; // Bruges KUN som startværdi nu
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

    // --- Interne Runtime Variabler ---
    private float currentBaseSpeed; // Den *faktiske aktuelle* grundhastighed (styres udefra)
    private float currentSpeed; // Hastighed lige nu (gå, sprint eller wheelchair speed)
    private CharacterController controller;
    private float velocityY;
    private bool isGrounded;

    // NYE: Til højde/center gendannelse
    private float originalHeight;
    private Vector3 originalCenter;

    // Kamera/Mus
    private float cameraCap;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;


    // Bevægelse
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;

    // Headbob
    private Vector3 initialHeadbobTargetLocalPos;
    private float bobTimer;

    private bool isTabletOpen = false;
    public bool IsSprint = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!controller) Debug.LogError("FATAL: Movement script mangler CharacterController!", this.gameObject);
        // ... (andre null tjek for camera, groundCheck etc.) ...
        if (playerCamera == null) Debug.LogError("FEJL: playerCamera er ikke tildelt!", this.gameObject);
        if (groundCheck == null) Debug.LogError("FEJL: groundCheck Transform er ikke tildelt!", this.gameObject);


        currentBaseSpeed = baseSpeed; // Initialiser med Inspector værdien
        currentSpeed = currentBaseSpeed;

        // --- NYT: Gem original højde og center ---
        originalHeight = controller.height;
        originalCenter = controller.center;
        Debug.Log($"Movement Start: Initial base speed={currentBaseSpeed:F1}, Original Height={originalHeight:F1}, Original Center={originalCenter}");

        // Headbob setup
        if (headbobTarget == null) headbobTarget = playerCamera;
        if (headbobTarget) initialHeadbobTargetLocalPos = headbobTarget.localPosition;

        UpdateCursorState();
    }

    void Update()
    {
        // AFGØRENDE TJEK: Gør intet hvis scriptet er deaktiveret
        if (!this.enabled)
        {
            currentDir = Vector2.zero; currentDirVelocity = Vector2.zero; return;
        }

        HandleGroundCheck();
        HandleMouseLook();
        HandleMovementInput();
        HandleGravityAndJump();
        ApplyMovement();
        if (enableHeadbob) ApplyHeadbob();
        HandleOtherKeybinds();
    }

    // --- Update Opdelte Funktioner --- (Stort set uændrede, men bruger currentBaseSpeed)

    void HandleGroundCheck() { isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer); }

    void HandleMouseLook()
    { /* ... uændret ... */
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

    void HandleMovementInput()
    { /* ... uændret logik, bruger nu currentBaseSpeed ... */
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        bool canSprint = staminaSystem == null || staminaSystem.HasStamina(0.1f);
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        bool isMovingEnough = currentDir.magnitude > 0.1f;

        // Sprint KUN hvis vi kan, vil, bevæger os, er på jorden OG base speed er over et lille threshold (ikke 0)
        if (wantsToSprint && isGrounded && isMovingEnough && canSprint && currentBaseSpeed > 0.1f)
        {

            currentSpeed = currentBaseSpeed * sprintSpeedMultiplier;
            staminaSystem?.UseStamina(staminaDrainPerSecond * Time.deltaTime);
        }
        else
        {
            currentSpeed = currentBaseSpeed; // Ellers brug den aktuelle base speed (som kan være wheelchair speed)

            velocityY = -2f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 moveVelocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed;
        moveVelocity.y = velocityY;

        controller.Move(moveVelocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        ApplyHeadbob();
    }

    void CheckKeybinds()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Brug (E) aktiveret");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload (R) aktiveret");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Drop (G) aktiveret");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Kast (Q) aktiveret");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Menu (ESC) open");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Lommelygte (F) tændt/slukket");
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Sigtning (Højreklik) aktiveret");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Skyd/Melee (Venstreklik) aktiveret");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTabletOpen = !isTabletOpen;
            Debug.Log(isTabletOpen ? "Tablet/iPad open" : "Tablet/iPad lukket");
        }

        // Sprinting logic
        if (Input.GetKey(KeyCode.LeftShift) && staminaSystem.HasStamina(1f) && currentDir.magnitude > 0.1f)
        {
            currentSpeed = 10.0f; // Sprint speed
            staminaSystem.UseStamina(Time.deltaTime * 20f); // Drain stamina per second
            IsSprint = true;
        }
        else
        {
            currentSpeed = baseSpeed; // Walking speed
            IsSprint = false;

        }
    }

    void HandleGravityAndJump()
    { /* ... uændret, men hop tjekker allerede base speed > 0 ... */
        if (isGrounded && velocityY < 0) velocityY = -2f;

        // Tjek for hop input - kræver at man er på jorden og IKKE har base speed 0 (eller meget lav)
        if (isGrounded && Input.GetButtonDown("Jump") && currentBaseSpeed > 0.1f)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocityY += gravity * Time.deltaTime;
    }

    void ApplyMovement()
    { /* ... uændret ... */
        Vector3 horizontalVelocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed;
        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = velocityY;
        controller?.Move(finalVelocity * Time.deltaTime);
    }

    void ApplyHeadbob()
    {
        if (!headbobTarget || controller == null) return;

        // Tjek om vi skal bobbe
        // Undgå division med nul hvis currentBaseSpeed er meget lav (wheelchair)
        bool canBob = isGrounded && controller.velocity.magnitude > 0.1f && currentSpeed > 0.1f && currentBaseSpeed > 0.1f;

        if (canBob)
        {
            // Beregn bob timer med skalering ift. base speed
            bobTimer += Time.deltaTime * bobFrequency * (currentSpeed / currentBaseSpeed);
            float bobY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;
            Vector3 bobOffset = new Vector3(bobX, bobY, 0f);
            headbobTarget.localPosition = Vector3.Lerp(headbobTarget.localPosition, initialHeadbobTargetLocalPos + bobOffset, Time.deltaTime * 10f);
        }
        else
        {
            // Gå tilbage til neutral position
            bobTimer = 0f;
            headbobTarget.localPosition = Vector3.Lerp(headbobTarget.localPosition, initialHeadbobTargetLocalPos, Time.deltaTime * 5f);
        }
    }

    void HandleOtherKeybinds()
    { /* ... uændret ... */
        // 'E' håndteres af SpinWheelController
        if (Input.GetKeyDown(KeyCode.R)) Debug.Log("Reload (R) aktiveret");
        // ... (resten af tasterne) ...
    }


    // -----===== PUBLIC METODER TIL STYRING UDEFRA =====-----

    public void UpdateCursorState()
    { /* ... uændret ... */
        if (cursorLock && this.enabled) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    }

    // Sætter den AKTUELLE grundhastighed
    public void SetSpeed(float newBaseSpeed)
    {
        currentBaseSpeed = Mathf.Max(0, newBaseSpeed);
        // Opdater straks 'currentSpeed' hvis vi ikke sprinter
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded && currentDir.magnitude > 0.1f;
        if (!isSprinting) { currentSpeed = currentBaseSpeed; }
        Debug.Log($"Movement - Base speed set to: {currentBaseSpeed:F1}");
    }

    // Returnerer den AKTUELLE grundhastighed
    public float GetBaseSpeed() { return currentBaseSpeed; }

    // Returnerer hastigheden LIGE NU
    public float GetCurrentActualSpeed() { return currentSpeed; }

    // --- NYE METODER TIL HØJDE/CENTER ---

    // Sætter CharacterController højde og justerer center automatisk
    public void SetCharacterHeightAndCenter(float newHeight)
    {
        if (controller == null) return;
        // Undgå negativ eller meget lille højde
        newHeight = Mathf.Max(0.1f, newHeight);
        // Juster controller højde
        controller.height = newHeight;
        // Juster center position (typisk halvdelen af højden op fra bunden)
        controller.center = new Vector3(originalCenter.x, newHeight / 2f, originalCenter.z); // Brug original X/Z
        Debug.Log($"Movement - Height set to {controller.height:F1}, Center set to {controller.center}");
    }

    // Gendanner den originale højde og center position gemt i Start()
    public void RestoreOriginalHeightAndCenter()
    {
        if (controller == null) return;
        controller.height = originalHeight;
        controller.center = originalCenter;
        Debug.Log($"Movement - Restored Original Height ({originalHeight:F1}) and Center ({originalCenter})");
    }

    // Returnerer den gemte originalhøjde (nyttig for debug eller UI)
    public float GetOriginalHeight()
    {
        return originalHeight;
    }

} // End of Movement class