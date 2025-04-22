using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float baseSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float jumpHeight = 6f;

    private float currentSpeed;
    private float velocityY;
    private bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    private bool isTabletOpen = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;

        UpdateCursorState();
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
        CheckKeybinds();
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = 10.0f;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("Næste weapon valgt");
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Debug.Log("Forrige weapon valgt");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Weapon 1 valgt");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Weapon 2 valgt");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Weapon 3 valgt");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Weapon 4 valgt");
        }
    }

    private void UpdateCursorState()
    {
        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !cursorLock;
    }
}
