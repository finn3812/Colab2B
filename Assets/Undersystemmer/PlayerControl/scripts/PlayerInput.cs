using UnityEngine;
using UnityEngine.SceneManagement; // Nødvendig for scene skift

// Håndterer spillerens input til at åbne/lukke for kamera-visningen (iPad'en)
public class PlayerInput : MonoBehaviour // Eller navnet på dit eksisterende input script
{
    [Tooltip("Navnet på den scene der indeholder iPad UI'en.")]
    public string cameraSceneName = "CameraScene"; // VIGTIGT: Skal matche navnet på din kamerascene!

    [Tooltip("Knappen der bruges til at åbne/lukke kamera-visningen.")]
    public KeyCode toggleCameraButton = KeyCode.Tab; // Du kan ændre knappen her

    private bool isCameraViewOpen = false; // Holder styr på om kameraerne er åbne lige nu

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

            // Er vi i hovedspillet og vil åbne kameraerne?
            if (currentSceneName != cameraSceneName && !isCameraViewOpen) // Dobbelttjek for at undgå fejl
            {
                // Før vi skifter: Tjek om SecurityCameraManager er klar
                if (SecurityCameraManager.Instance != null)
                {
                    Debug.Log($"Åbner kamera visning. Skifter til scene: {cameraSceneName}");
                    SceneManager.LoadScene(cameraSceneName);
                    isCameraViewOpen = true; // Marker at vi har åbnet den (selvom scenen skifter lige om lidt)
                }
                else
                {
                    Debug.LogError("Kan ikke åbne kamera visning: SecurityCameraManager blev ikke fundet! Sørg for den er i MainGameScene.");
                }
            }
            // Er vi i kameravisningen og vil lukke den?
            // (Bemærk: IPadController håndterer også lukning, dette er en ekstra sikkerhed/alternativ)
            else if (currentSceneName == cameraSceneName && isCameraViewOpen)
            {
                // IPadController burde håndtere dette via dens egen Update(),
                // men hvis dette script af en eller anden grund stadig kører (f.eks. hvis det også er DontDestroyOnLoad),
                // kunne man tilføje logik her. Typisk er det dog IPadController der lukker.
                // Man kunne kalde en metode på en IPadController instance hvis man havde en reference.
                // For nu lader vi IPadController klare lukningen.
                Debug.Log("PlayerInput: Forsøger at lukke kamera fra MainGameScene's input script - dette bør håndteres af IPadController.");
            }
        }
    }

    // Denne metode kaldes automatisk af Unity når en ny scene er færdig med at loade
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
        // Opdater status baseret på den nye scene
        isCameraViewOpen = (scene.name == cameraSceneName);
        Debug.Log($"Scene '{scene.name}' loaded. isCameraViewOpen = {isCameraViewOpen}");
    }
}