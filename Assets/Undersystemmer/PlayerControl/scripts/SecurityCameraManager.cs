using UnityEngine;
using UnityEngine.Rendering; // Ikke strengt n�dvendigt for denne version, men god at have hvis man udvider

// Sikrer at der kun er �n instans af denne manager og at den overlever scene skift
// Holder referencer til de Render Textures som sikkerhedskameraerne skriver til.
public class SecurityCameraManager : MonoBehaviour
{
    // Singleton instance for nem adgang fra andre scripts/scener
    public static SecurityCameraManager Instance { get; private set; }

    // Tr�k dine 4 Render Textures herind i Inspectoren
    // R�kkef�lgen her bestemmer hvilket feed der vises p� hvilket display i IPadControlleren
    [Tooltip("Tr�k de 4 Render Texture assets herind. R�kkef�lgen betyder noget!")]
    public RenderTexture[] cameraFeeds = new RenderTexture[4];

    // (Valgfrit) Tr�k dine sikkerhedskameraer herind, hvis du vil styre dem herfra
    // public Camera[] securityCameras = new Camera[4];

    // (Valgfrit) Liste over mulige spawn-punkter for kameraerne
    // public Transform[] possibleCameraLocations;

    void Awake()
    {
        // --- Singleton Ops�tning ---
        // Tjekker om der allerede findes en instance
        if (Instance != null && Instance != this)
        {
            // Hvis ja, og det ikke er denne, s� �del�g denne nye GameObject
            Debug.LogWarning("En anden SecurityCameraManager instance fundet. �del�gger denne.");
            Destroy(gameObject);
            return; // Stop videre udf�relse af Awake for denne kopi
        }
        else
        {
            // Ellers, s�t denne som den globale instance
            Instance = this;
            // S�rg for at denne manager ikke �del�gges, n�r vi skifter scene
            // Dette er VIGTIGT for at IPadController kan finde den i den anden scene.
            DontDestroyOnLoad(gameObject);
            Debug.Log("SecurityCameraManager initialiseret og sat til DontDestroyOnLoad.");
        }
        // --- Slut Singleton ---


        // Simpelt tjek for at sikre at antallet af textures er som forventet
        if (cameraFeeds.Length != 4)
        {
            Debug.LogError("SecurityCameraManager: Der skal v�re pr�cis 4 Render Textures tildelt i Inspectoren!");
        }
        else
        {
            // Tjekker om alle pladser i arrayet faktisk er udfyldt
            for (int i = 0; i < cameraFeeds.Length; i++)
            {
                if (cameraFeeds[i] == null)
                {
                    Debug.LogError($"SecurityCameraManager: Plads {i} i cameraFeeds arrayet er tom (null)! Tr�k en Render Texture ind.");
                }
            }
        }

        // --- Valgfri: Tilf�ldig Placering (fra tidligere eksempel) ---
        // Afkommenter og brug hvis du har sat 'securityCameras' og 'possibleCameraLocations' op
        /*
        if (securityCameras.Length == 4 && possibleCameraLocations != null && possibleCameraLocations.Length >= 4)
        {
            PlaceCamerasRandomly();
        }
        */
        // --- Slut Valgfri ---
    }

    // --- Valgfri: Tilf�ldig Placering Funktion (fra tidligere eksempel) ---
    /*
    void PlaceCamerasRandomly()
    {
        // ... (koden for funktionen her) ...
    }
    */
    // --- Slut Valgfri Funktion ---
}