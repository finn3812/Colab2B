using UnityEngine;
using UnityEngine.SceneManagement;

public class SecurityCameraManager : MonoBehaviour
{
    public static SecurityCameraManager Instance { get; private set; }
    public RenderTexture[] cameraFeeds = new RenderTexture[4];

    private static Vector3 s_lastPlayerPosition;
    private static Quaternion s_lastPlayerRotation;
    private static bool s_hasSavedPosition = false;
    private static string s_scenePlayerWasIn = ""; // Store the scene name we saved from

    [Header("Player Persistence & Scene Settings")]
    [Tooltip("The tag assigned to your player GameObject in the Main Game Scene.")]
    public string playerTag = "Player";

    [Tooltip("Navnet på din hovedspilscene (hvor spilleren er).")]
    public string mainGameSceneName = "MainGameScene";

    [Tooltip("Navnet på din kamera/iPad scene.")]
    public string cameraSceneName = "CameraScene";

    [Tooltip("Tasten der bruges til at åbne iPad'en fra hovedspilscenen.")]
    public KeyCode openIPadKey = KeyCode.E;

    void Awake()
    {
        Debug.Log("[SCM] Awake called.");
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[SCM] Another SecurityCameraManager instance found. Destroying this new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[SCM] Initialized and set to DontDestroyOnLoad.");

        // Inspector value checks
        if (string.IsNullOrEmpty(mainGameSceneName)) Debug.LogError("[SCM] 'Main Game Scene Name' is NOT SET in Inspector!");
        else Debug.Log($"[SCM] Main Game Scene Name: '{mainGameSceneName}'");

        if (string.IsNullOrEmpty(cameraSceneName)) Debug.LogError("[SCM] 'Camera Scene Name' is NOT SET in Inspector!");
        else Debug.Log($"[SCM] Camera Scene Name: '{cameraSceneName}'");

        if (string.IsNullOrEmpty(playerTag)) Debug.LogError("[SCM] 'Player Tag' is NOT SET in Inspector!");
        else Debug.Log($"[SCM] Player Tag: '{playerTag}'");

        if (cameraFeeds.Length != 4) Debug.LogError("[SCM] Expected 4 Render Textures, check Inspector!");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("[SCM] Subscribed to SceneManager.sceneLoaded.");
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("[SCM] Unsubscribed from SceneManager.sceneLoaded.");
    }

    void Update()
    {
        // Only check for input if we are in the main game scene
        if (SceneManager.GetActiveScene().name == mainGameSceneName)
        {
            if (Input.GetKeyDown(openIPadKey))
            {
                Debug.Log($"[SCM] '{openIPadKey}' pressed in scene '{SceneManager.GetActiveScene().name}'. Attempting to open iPad view.");
                OpenIPadAndSavePlayerPosition();
            }
        }
    }

    public void OpenIPadAndSavePlayerPosition()
    {
        Debug.Log("[SCM] OpenIPadAndSavePlayerPosition() called.");
        if (string.IsNullOrEmpty(playerTag))
        {
            Debug.LogError("[SCM] Player Tag is not set. Cannot save position.");
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            s_lastPlayerPosition = playerObject.transform.position;
            s_lastPlayerRotation = playerObject.transform.rotation;
            s_hasSavedPosition = true;
            s_scenePlayerWasIn = SceneManager.GetActiveScene().name; // Record the scene we are leaving
            Debug.Log($"[SCM] Player FOUND with tag '{playerTag}'. Position saved: {s_lastPlayerPosition}, Rotation: {s_lastPlayerRotation.eulerAngles}. From scene: '{s_scenePlayerWasIn}'. s_hasSavedPosition is NOW TRUE.");

            if (string.IsNullOrEmpty(cameraSceneName))
            {
                Debug.LogError("[SCM] 'Camera Scene Name' is not set in Inspector! Cannot switch scene.");
                return;
            }
            Debug.Log($"[SCM] Loading camera scene: '{cameraSceneName}'");
            SceneManager.LoadScene(cameraSceneName);
        }
        else
        {
            Debug.LogError($"[SCM] Player NOT FOUND with tag '{playerTag}' in current scene '{SceneManager.GetActiveScene().name}'. Cannot save position or open iPad.");
            s_hasSavedPosition = false; // Ensure it's false if we didn't save
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[SCM] OnSceneLoaded: Loaded scene '{scene.name}' (Mode: {mode}). Target main game scene: '{mainGameSceneName}'.");
        Debug.Log($"[SCM] OnSceneLoaded: Current s_hasSavedPosition = {s_hasSavedPosition}. Scene player was in: '{s_scenePlayerWasIn}'");

        // Check if we are returning to the main game scene AND a position was saved AND we came from that main game scene
        if (scene.name == mainGameSceneName && s_hasSavedPosition && s_scenePlayerWasIn == mainGameSceneName)
        {
            Debug.Log($"[SCM] Conditions MET to restore player position in '{scene.name}'.");

            if (string.IsNullOrEmpty(playerTag))
            {
                Debug.LogError("[SCM] Player Tag is not set in Inspector. Cannot find player to restore position.");
                s_hasSavedPosition = false; // Don't try again without a tag
                s_scenePlayerWasIn = "";
                return;
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject != null)
            {
                Debug.Log($"[SCM] Player FOUND in '{scene.name}' with tag '{playerTag}': '{playerObject.name}'. Current pos: {playerObject.transform.position}. Attempting to restore to {s_lastPlayerPosition}");
                playerObject.transform.position = s_lastPlayerPosition;
                playerObject.transform.rotation = s_lastPlayerRotation;
                Debug.Log($"[SCM] Player position RESTORED to: {playerObject.transform.position}, Rotation: {playerObject.transform.rotation.eulerAngles}");

                // Verify immediately after setting
                if (Vector3.Distance(playerObject.transform.position, s_lastPlayerPosition) > 0.01f)
                {
                    Debug.LogError($"[SCM] !!! POSITION MISMATCH AFTER SETTING !!! Player is at {playerObject.transform.position} but should be {s_lastPlayerPosition}. Something might be overriding it immediately!");
                }

                s_hasSavedPosition = false; // Reset the flag, restoration attempt complete
                s_scenePlayerWasIn = "";    // Reset the scene origin
                Debug.Log("[SCM] s_hasSavedPosition reset to FALSE. Restoration complete.");
            }
            else
            {
                Debug.LogWarning($"[SCM] Player NOT FOUND with tag '{playerTag}' in scene '{scene.name}' upon return. Position NOT restored. s_lastPlayerPosition was {s_lastPlayerPosition}");
                // Decide if you want to keep s_hasSavedPosition true for another try, or false.
                // For now, setting to false as the current scene load didn't find it.
                s_hasSavedPosition = false;
                s_scenePlayerWasIn = "";
            }
        }
        else
        {
            Debug.Log("[SCM] Conditions NOT MET for player position restoration. Reasons:");
            if (scene.name != mainGameSceneName) Debug.Log($"  - Loaded scene '{scene.name}' is not the target main game scene '{mainGameSceneName}'.");
            if (!s_hasSavedPosition) Debug.Log("  - s_hasSavedPosition is FALSE.");
            if (s_scenePlayerWasIn != mainGameSceneName && s_hasSavedPosition) Debug.Log($"  - Player was last saved from scene '{s_scenePlayerWasIn}', not necessarily returning to '{mainGameSceneName}' in the expected context for restoration.");

            // If we load the main game scene for any other reason (e.g. initial game start),
            // ensure any stale saved position flags are cleared.
            if (scene.name == mainGameSceneName && !s_hasSavedPosition)
            { // Or if conditions above just failed
                Debug.Log($"[SCM] Loaded main game scene ('{scene.name}') but not in restoration context, or restoration failed. Ensuring s_hasSavedPosition is false.");
                s_hasSavedPosition = false;
                s_scenePlayerWasIn = "";
            }
        }
    }
}