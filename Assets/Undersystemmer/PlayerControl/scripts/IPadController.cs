using UnityEngine;
using UnityEngine.UI; // Nødvendig for RawImage
using UnityEngine.SceneManagement; // Nødvendig for scene skift

public class IPadController : MonoBehaviour
{
    // Træk de 4 RawImage elementer herind fra Hierarchy'en i CameraScene
    [Tooltip("Træk de 4 RawImage UI elementer herind, som skal vise kamerafeeds.")]
    public RawImage[] cameraDisplays = new RawImage[4];

    // Navnet på din hovedspilscene
    [Tooltip("Navnet på scenen der skal skiftes tilbage til (din hovedspilscene).")]
    public string mainGameSceneName = "MainGameScene";

    void Start()
    {
        // Tjek om SecurityCameraManager eksisterer og har textures klar
        if (SecurityCameraManager.Instance != null && SecurityCameraManager.Instance.cameraFeeds != null)
        {
            RenderTexture[] feeds = SecurityCameraManager.Instance.cameraFeeds;

            // Sikrer at vi har nok displays og feeds at arbejde med
            int displayCount = Mathf.Min(cameraDisplays.Length, feeds.Length);
            Debug.Log($"Forsøger at tildele {displayCount} kamerafeeds til displays.");

            for (int i = 0; i < displayCount; i++)
            {
                // Tildel den korrekte Render Texture til hver RawImage
                if (cameraDisplays[i] != null && feeds[i] != null)
                {
                    cameraDisplays[i].texture = feeds[i];
                    cameraDisplays[i].enabled = true; // Sørg for at billedet er synligt
                    Debug.Log($"Tildelte feed {i} til display {i}.");
                }
                else
                {
                    Debug.LogWarning($"IPadController: Mangler RawImage display eller RenderTexture feed for index {i}.");
                    if (cameraDisplays[i] != null)
                    {
                        cameraDisplays[i].enabled = false; // Skjul hvis data mangler
                    }
                }
            }

            // Skjul eventuelle overskydende displays hvis der er flere displays end feeds
            for (int i = displayCount; i < cameraDisplays.Length; i++)
            {
                if (cameraDisplays[i] != null)
                {
                    cameraDisplays[i].enabled = false;
                    Debug.Log($"Deaktiverede ubrugt display index {i}.");
                }
            }
        }
        else
        {
            Debug.LogError("IPadController: Kunne ikke finde SecurityCameraManager instance eller dens cameraFeeds! Kamerafeeds vil ikke blive vist.");
            // Skjul alle displays hvis manageren ikke findes
            foreach (var display in cameraDisplays)
            {
                if (display != null) display.enabled = false;
            }
        }
    }

    void Update()
    {
        // Tjek for input for at lukke iPad'en igen (f.eks. Tab eller Escape)
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseIPad();
        }
    }

    // Funktion til at lukke iPad'en og vende tilbage til spillet
    public void CloseIPad()
    {
        // Sørg for at mainGameSceneName er sat korrekt i Inspectoren!
        if (string.IsNullOrEmpty(mainGameSceneName))
        {
            Debug.LogError("Main Game Scene Name er ikke sat i IPadController! Kan ikke skifte scene.");
            return;
        }
        Debug.Log($"Lukker iPad og skifter tilbage til scene: {mainGameSceneName}");
        SceneManager.LoadScene(mainGameSceneName);
        // SecurityCameraManager (med DontDestroyOnLoad) vil stadig eksistere.
    }

    // Valgfrit: Du kan tilføje en UI Knap på din iPad, der kalder CloseIPad()
    // Se instruktioner i den oprindelige guide.
}