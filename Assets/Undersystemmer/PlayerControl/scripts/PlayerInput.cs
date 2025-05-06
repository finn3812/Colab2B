using UnityEngine;
using UnityEngine.SceneManagement; // N�dvendig for scene skift

// H�ndterer spillerens input til at �bne/lukke for kamera-visningen (iPad'en)
public class PlayerInput : MonoBehaviour // Eller navnet p� dit eksisterende input script
{
    [Tooltip("Navnet p� den scene der indeholder iPad UI'en.")]
    public string cameraSceneName = "CameraScene"; // VIGTIGT: Skal matche navnet p� din kamerascene!

    [Tooltip("Knappen der bruges til at �bne/lukke kamera-visningen.")]
    public KeyCode toggleCameraButton = KeyCode.Tab; // Du kan �ndre knappen her

    private bool isCameraViewOpen = false; // Holder styr p� om kameraerne er �bne lige nu

    void Update()
    {
        CheckForCameraToggle();
    }

    void CheckForCameraToggle()
    {
        // Tjekker om den definerede knap trykkes ned
        if (Input.GetKeyDown(toggleCameraButton))
        {
            // Find ud af hvilken scene der er aktiv lige nu
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Er vi i hovedspillet og vil �bne kameraerne?
            if (currentSceneName != cameraSceneName && !isCameraViewOpen) // Dobbelttjek for at undg� fejl
            {
                // F�r vi skifter: Tjek om SecurityCameraManager er klar
                if (SecurityCameraManager.Instance != null)
                {
                    Debug.Log($"�bner kamera visning. Skifter til scene: {cameraSceneName}");
                    SceneManager.LoadScene(cameraSceneName);
                    isCameraViewOpen = true; // Marker at vi har �bnet den (selvom scenen skifter lige om lidt)
                }
                else
                {
                    Debug.LogError("Kan ikke �bne kamera visning: SecurityCameraManager blev ikke fundet! S�rg for den er i MainGameScene.");
                }
            }
            // Er vi i kameravisningen og vil lukke den?
            // (Bem�rk: IPadController h�ndterer ogs� lukning, dette er en ekstra sikkerhed/alternativ)
            else if (currentSceneName == cameraSceneName && isCameraViewOpen)
            {
                // IPadController burde h�ndtere dette via dens egen Update(),
                // men hvis dette script af en eller anden grund stadig k�rer (f.eks. hvis det ogs� er DontDestroyOnLoad),
                // kunne man tilf�je logik her. Typisk er det dog IPadController der lukker.
                // Man kunne kalde en metode p� en IPadController instance hvis man havde en reference.
                // For nu lader vi IPadController klare lukningen.
                Debug.Log("PlayerInput: Fors�ger at lukke kamera fra MainGameScene's input script - dette b�r h�ndteres af IPadController.");
            }
        }
    }

    // Denne metode kaldes automatisk af Unity n�r en ny scene er f�rdig med at loade
    // Vi bruger den til at opdatere vores 'isCameraViewOpen' flag
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Opdater status baseret p� den nye scene
        isCameraViewOpen = (scene.name == cameraSceneName);
        Debug.Log($"Scene '{scene.name}' loaded. isCameraViewOpen = {isCameraViewOpen}");
    }
}