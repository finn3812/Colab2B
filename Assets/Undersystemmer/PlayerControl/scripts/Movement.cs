using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{
    [Header("StaminaSlideBar")]
    [SerializeField] StaminaSystem staminaSystem;

    [Header("Camera & Mouse")]
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;

    [Header("Movement")]
    [SerializeField] float baseSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float jumpHeight = 6f;

    [Header("Headbob Settings")]
    [SerializeField] float bobFrequency = 1.5f;
    [SerializeField] float bobAmplitude = 0.05f;
    [SerializeField] float bobSpeedMultiplier = 1.0f;

    private Vector3 initialCameraLocalPos;
    private float bobTimer;

    private float currentSpeed;
    private float velocityY;
    private bool isGrounded;

    private float cameraCap;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    private CharacterController controller;
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;

    private bool isTabletOpen = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;
        initialCameraLocalPos = playerCamera.localPosition;
        UpdateCursorState();
    }

    void Update()
    {
        UpdateMouse();
        CheckKeybinds(); // Make sure this is before UpdateMove()
        UpdateMove();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (isGrounded && velocityY < 0)
        {
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
        if (Input.GetKeyDown(KeyCode.E)) Debug.Log("Brug (E) aktiveret");
        if (Input.GetKeyDown(KeyCode.R)) Debug.Log("Reload (R) aktiveret");
        if (Input.GetKeyDown(KeyCode.G)) Debug.Log("Drop (G) aktiveret");
        if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Kast (Q) aktiveret");
        if (Input.GetKeyDown(KeyCode.Escape)) Debug.Log("Menu (ESC) open");
        if (Input.GetKeyDown(KeyCode.F)) Debug.Log("Lommelygte (F) tændt/slukket");
        if (Input.GetMouseButtonDown(1)) Debug.Log("Sigtning (Højreklik) aktiveret");
        if (Input.GetMouseButtonDown(0)) Debug.Log("Skyd/Melee (Venstreklik) aktiveret");

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTabletOpen = !isTabletOpen;
            Debug.Log(isTabletOpen ? "Tablet/iPad open" : "Tablet/iPad lukket");
        }

        if (Input.GetKey(KeyCode.LeftShift) && staminaSystem.HasStamina(1f) && currentDir.y > 0)
        {
            currentSpeed = 10.0f;
            staminaSystem.UseStamina(Time.deltaTime * 20f);
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) Debug.Log("Næste weapon valgt");
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) Debug.Log("Forrige weapon valgt");

        if (Input.GetKeyDown(KeyCode.Alpha1)) Debug.Log("Weapon 1 valgt");
        if (Input.GetKeyDown(KeyCode.Alpha2)) Debug.Log("Weapon 2 valgt");
        if (Input.GetKeyDown(KeyCode.Alpha3)) Debug.Log("Weapon 3 valgt");
        if (Input.GetKeyDown(KeyCode.Alpha4)) Debug.Log("Weapon 4 valgt");
    }

    void ApplyHeadbob()
    {
        if (isGrounded && currentDir.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency * currentSpeed * bobSpeedMultiplier;

            float bobY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobX = Mathf.Cos(bobTimer * 2f) * bobAmplitude * 0.5f;
            float bobZ = Mathf.Sin(bobTimer * 2f) * bobAmplitude * 0.2f;

            float sprintMultiplier = currentSpeed > baseSpeed ? 1.5f : 1f;
            Vector3 bobPosition = initialCameraLocalPos + new Vector3(bobX, bobY, bobZ) * sprintMultiplier;

            playerCamera.localPosition = bobPosition;
        }
        else
        {
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, initialCameraLocalPos, Time.deltaTime * 5f);
            bobTimer = 0f;
        }
    }

    private void UpdateCursorState()
    {
        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLock;
    }
}