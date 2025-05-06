using UnityEngine;
using UnityEngine.Rendering; // Ikke strengt nødvendigt for denne version, men god at have hvis man udvider

// Sikrer at der kun er én instans af denne manager og at den overlever scene skift
// Holder referencer til de Render Textures som sikkerhedskameraerne skriver til.
public class SecurityCameraManager : MonoBehaviour
{
    // Singleton instance for nem adgang fra andre scripts/scener
    public static SecurityCameraManager Instance { get; private set; }

    // Træk dine 4 Render Textures herind i Inspectoren
    // Rækkefølgen her bestemmer hvilket feed der vises på hvilket display i IPadControlleren
    [Tooltip("Træk de 4 Render Texture assets herind. Rækkefølgen betyder noget!")]
    public RenderTexture[] cameraFeeds = new RenderTexture[4];

    // (Valgfrit) Træk dine sikkerhedskameraer herind, hvis du vil styre dem herfra
    // public Camera[] securityCameras = new Camera[4];

    // (Valgfrit) Liste over mulige spawn-punkter for kameraerne
    // public Transform[] possibleCameraLocations;

    void Awake()
    {
        // --- Singleton Opsætning ---
        // Tjekker om der allerede findes en instance
        if (Instance != null && Instance != this)
        {
            // Hvis ja, og det ikke er denne, så ødelæg denne nye GameObject
            Debug.LogWarning("En anden SecurityCameraManager instance fundet. Ødelægger denne.");
            Destroy(gameObject);
            return; // Stop videre udførelse af Awake for denne kopi
        }
        else
        {
            // Ellers, sæt denne som den globale instance
            Instance = this;
            // Sørg for at denne manager ikke ødelægges, når vi skifter scene
            // Dette er VIGTIGT for at IPadController kan finde den i den anden scene.
            DontDestroyOnLoad(gameObject);
            Debug.Log("SecurityCameraManager initialiseret og sat til DontDestroyOnLoad.");
        }
        // --- Slut Singleton ---


        // Simpelt tjek for at sikre at antallet af textures er som forventet
        if (cameraFeeds.Length != 4)
        {
            Debug.LogError("SecurityCameraManager: Der skal være præcis 4 Render Textures tildelt i Inspectoren!");
        }
        else
        {
            // Tjekker om alle pladser i arrayet faktisk er udfyldt
            for (int i = 0; i < cameraFeeds.Length; i++)
            {
                if (cameraFeeds[i] == null)
                {
                    Debug.LogError($"SecurityCameraManager: Plads {i} i cameraFeeds arrayet er tom (null)! Træk en Render Texture ind.");
                }
            }
        }

        // --- Valgfri: Tilfældig Placering (fra tidligere eksempel) ---
        // Afkommenter og brug hvis du har sat 'securityCameras' og 'possibleCameraLocations' op
        /*
        if (securityCameras.Length == 4 && possibleCameraLocations != null && possibleCameraLocations.Length >= 4)
        {
            PlaceCamerasRandomly();
        }
        */
        // --- Slut Valgfri ---
    }

    // --- Valgfri: Tilfældig Placering Funktion (fra tidligere eksempel) ---
    /*
    void PlaceCamerasRandomly()
    {
        // ... (koden for funktionen her) ...
    }
    */
    // --- Slut Valgfri Funktion ---
}