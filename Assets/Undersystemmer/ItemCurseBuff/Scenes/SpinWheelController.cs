using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinWheelController : MonoBehaviour
{
    [Header("UI & Data Referencer")]
    [Tooltip("Billedet af selve lykkehjulet")]
    public Image SpinWheelImg;
    [Tooltip("Scriptable Object der indeholder hjulets grafik")]
    public SpinWheelImageSO SpinWheelData;
    [Tooltip("Knappen der starter spin")]
    public Button SpinBtn;
    [Tooltip("Hele Canvas GameObject der indeholder hjulet")]
    public GameObject SpinCanvas;
    [Tooltip("Tekstfelt til at vise spillerens stats")]
    public TMP_Text StatsText;
    [Tooltip("Slider UI element til Gambling Meter")]
    public Slider GamblingMeterUI; // Valgfri

    [Header("Spiller & Effekt Referencer")]
    [Tooltip("Reference til spillerens Movement script")]
    public Movement playerMovement; // !! VIGTIGT: Skal tildeles i Inspector !!
    [Tooltip("GameObject der evt. skal skifte farve ved effekt")]
    public GameObject AffectedObject; // Valgfrit

    [Header("Wheelchair Effekt Settings")] // NY SEKTION
    [Tooltip("Hastigheden spilleren skal have når de er i kørestol")]
    [SerializeField] float wheelchairSpeed = 2.0f;
    [Tooltip("Højden spillerens CharacterController skal have når de er i kørestol")]
    [SerializeField] float wheelchairHeight = 1.0f; // Juster denne værdi efter spillerens model

    // Wheel Manager & State
    private SpinWheelManager spinWheelManager;
    private bool isCanvasOpen = false;

    // Interne Spiller Stats (Styret af denne controller)
    private int health = 100;
    // Holder styr på den NUVÆRENDE IKKE-kørestols base speed
    private float currentPlayerBaseSpeed = 5f; // Default værdi, overskrives i Start()
    private float blindness = 0f;
    private bool wheelchairBound = false; // Er spilleren i kørestol LIGE NU?
    private float stealth = 0f;
    private bool gamblingMeterActive = false;
    private float energyDepletion = 0f;
    private float gamblingMeterValue = 0f;
    private float gamblingIncreaseRate = 5f;

    // Andet
    private Renderer objectRenderer; // Til farveændring

    void Start()
    {
        // --- Robust Fejlhåndtering af Referencer ---
        bool setupError = false;
        if (playerMovement == null) { Debug.LogError("FATAL FEJL: 'playerMovement' er IKKE tildelt!", this.gameObject); setupError = true; }
        if (SpinCanvas == null) { Debug.LogError("FATAL FEJL: 'SpinCanvas' er IKKE tildelt!", this.gameObject); setupError = true; }
        if (SpinBtn == null) { Debug.LogError("FEJL: 'SpinBtn' er ikke tildelt!", this.gameObject); /* Kan fortsætte? */ }
        if (SpinWheelImg == null) Debug.LogError("FEJL: 'SpinWheelImg' er ikke tildelt!", this.gameObject);
        if (SpinWheelData == null) Debug.LogError("FEJL: 'SpinWheelData' (SO) er ikke tildelt!", this.gameObject);
        if (StatsText == null) Debug.LogWarning("Advarsel: 'StatsText' er ikke tildelt.", this.gameObject);
        if (GamblingMeterUI == null) Debug.LogWarning("Advarsel: 'GamblingMeterUI' er ikke tildelt.", this.gameObject);

        if (setupError) { this.enabled = false; return; }
        // --- Slut Fejlhåndtering ---

        // Gem spillerens *start* base speed VED SPILLETS START
        // Og sørg for at Movement scriptet kender sin originale højde
        currentPlayerBaseSpeed = playerMovement.GetBaseSpeed();
        // Vi behøver ikke gemme original højde her, Movement styrer det selv
        Debug.Log($"SpinWheelController Start: Initial non-wheelchair base speed={currentPlayerBaseSpeed:F1}, original height={playerMovement.GetOriginalHeight():F1}");

        // Initialiser Spin Wheel Manager
        spinWheelManager = new SpinWheelManager(SpinWheelImg, SpinWheelData, RotateWheel, HandleSegmentHit);
        if (SpinBtn != null) SpinBtn.onClick.AddListener(StartSpin);

        // Opsætning af UI
        SpinCanvas.SetActive(false);
        isCanvasOpen = false;
        if (AffectedObject != null) objectRenderer = AffectedObject.GetComponent<Renderer>();
        if (GamblingMeterUI != null) { /*...*/ GamblingMeterUI.gameObject.SetActive(false); }

        UpdateStatsUI(); // Opdater UI med start-værdier
    }

    void Update()
    {
        // Lyt efter 'E' for at åbne/lukke canvas
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSpinWheelCanvas();
        }

        // Opdater hjulet hvis det spinner
        if (isCanvasOpen && spinWheelManager != null)
        {
            spinWheelManager.UpdateSpin(Time.deltaTime);
        }

        // Opdater gambling meter hvis aktivt
        if (gamblingMeterActive && GamblingMeterUI != null && GamblingMeterUI.gameObject.activeSelf)
        {
            gamblingMeterValue += gamblingIncreaseRate * Time.deltaTime;
            gamblingMeterValue = Mathf.Clamp(gamblingMeterValue, GamblingMeterUI.minValue, GamblingMeterUI.maxValue);
            GamblingMeterUI.value = gamblingMeterValue;
            UpdateStatsUI(); // Opdater også stats teksten
        }
    }

    // Åbner/lukker for spin hjul canvas og håndterer movement/cursor
    void ToggleSpinWheelCanvas()
    {
        isCanvasOpen = !isCanvasOpen;
        SpinCanvas.SetActive(isCanvasOpen);

        if (isCanvasOpen)
        { // ÅBNER
            if (playerMovement != null) playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        else
        { // LUKKER
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                playerMovement.UpdateCursorState();
            }
            else { /* Fallback cursor lock */ Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        }
    }

    // Kaldes når Spin knappen trykkes
    private void StartSpin()
    {
        if (spinWheelManager != null && !spinWheelManager.IsSpinning())
        {
            Debug.Log("Starting spin...");
            spinWheelManager.StartSpin(3000f);
            ResetGamblingMeter();
        }
        else if (spinWheelManager != null && spinWheelManager.IsSpinning())
        {
            Debug.Log("Wheel is already spinning!");
        }
        else { Debug.LogError("Cannot start spin: spinWheelManager is null!"); }
    }

    // --- Callback funktioner til SpinWheelManager ---

    // Roterer hjulets billede
    private void RotateWheel(float rotationAmount)
    {
        if (SpinWheelImg != null)
        {
            SpinWheelImg.rectTransform.Rotate(0, 0, -rotationAmount);
        }
    }

    // Håndterer resultatet når hjulet stopper
    private void HandleSegmentHit(int segment)
    {
        Debug.Log($"--- Wheel landed on segment: {segment} ---");
        if (playerMovement == null)
        {
            Debug.LogError("FATAL: Cannot apply effects, playerMovement reference missing!");
            return;
        }

        // --- STEP 1: GENDAN FRA WHEELCHAIR (HVIS AKTIV) ---
        // Dette sker FØR den nye effekt påføres, og sikrer at
        // spilleren er i "normal" tilstand når næste switch-case køres.
        if (wheelchairBound)
        {
            Debug.Log($"Removing wheelchair: Restoring speed to {currentPlayerBaseSpeed:F1} and original height.");
            // Gendan IKKE-kørestols hastighed (den vi har gemt)
            playerMovement.SetSpeed(currentPlayerBaseSpeed);
            // Gendan original controller højde og center
            playerMovement.RestoreOriginalHeightAndCenter(); // NY METODE I MOVEMENT
            wheelchairBound = false; // Sluk for wheelchair flaget
        }
        // TODO: Nulstil evt. andre effekter (f.eks. blindness overlay) her

        // --- STEP 2: ANVEND NY EFFEKT ---
        switch (segment)
        {
            case 0: // HP+
                health = Mathf.Max(0, health + 20);
                Debug.Log($"Buff: Health increased! Current HP: {health}");
                ChangeObjectColor(Color.red);
                break;

            case 1: // Wheelchair (NY LOGIK)
                // Nulstilling skete ovenfor. Nu anvender vi den nye state.
                wheelchairBound = true; // Sæt flaget
                playerMovement.SetSpeed(wheelchairSpeed); // Sæt til specifik kørestolshastighed
                playerMovement.SetCharacterHeightAndCenter(wheelchairHeight); // Sæt til kørestolshøjde (NY METODE)
                Debug.Log($"Curse: Wheelchair Bound! Player speed set to {wheelchairSpeed:F1}, height set to {wheelchairHeight:F1}.");
                ChangeObjectColor(Color.gray);
                break;

            case 2: // Speed+
                float oldBaseSpeed = playerMovement.GetBaseSpeed();
                float newCalculatedBaseSpeed = oldBaseSpeed + 2f;
                playerMovement.SetSpeed(newCalculatedBaseSpeed);
                // !! VIGTIGT: Opdater den gemte IKKE-kørestols hastighed !!
                currentPlayerBaseSpeed = newCalculatedBaseSpeed;
                Debug.Log($"Buff: Speed Up! Non-wheelchair base speed now: {currentPlayerBaseSpeed:F1}");
                ChangeObjectColor(Color.yellow);
                break;

            case 3: // Gambling Meter
                if (!gamblingMeterActive)
                {
                    gamblingMeterActive = true;
                    if (GamblingMeterUI != null) GamblingMeterUI.gameObject.SetActive(true);
                    Debug.Log("Curse: Gambling Meter ACTIVATED!");
                }
                else { Debug.Log("Curse: Gambling Meter (already active)."); }
                ChangeObjectColor(Color.green);
                break;

            case 4: // Stealth+
                stealth = Mathf.Clamp(stealth + 25f, 0, 100);
                Debug.Log($"Buff: Stealth increased! Current Stealth: {stealth}%");
                ChangeObjectColor(Color.blue);
                // TODO: Implementer stealth effekt
                break;

            case 5: // Blindness+
                blindness = Mathf.Clamp(blindness + 50f, 0, 100);
                Debug.Log($"Curse: Blindness increased! Current Blindness: {blindness}%");
                ChangeObjectColor(new Color(0.5f, 0f, 0.5f));
                // TODO: Implementer visuel blindness effekt
                break;

            case 6: // Energy Depletion+
                energyDepletion = Mathf.Clamp(energyDepletion + 30f, 0, 100);
                Debug.Log($"Curse: Energy Depletion increased! Current Depletion: {energyDepletion}%");
                ChangeObjectColor(Color.magenta);
                // TODO: Implementer energy depletion effekt (fx på stamina regen)
                break;

            default:
                Debug.LogWarning($"Ukendt segment ramt: {segment}. Ingen effekt anvendt.");
                break;
        }

        // Opdater ALTID UI efter en effekt er håndteret
        UpdateStatsUI();
    }

    // --- Hjælpefunktioner ---

    private void ResetGamblingMeter()
    {
        if (gamblingMeterActive && GamblingMeterUI != null)
        {
            gamblingMeterValue = 0;
            GamblingMeterUI.value = gamblingMeterValue;
            Debug.Log("Gambling meter reset to 0.");
        }
    }

    private void ChangeObjectColor(Color newColor)
    {
        if (objectRenderer == null && AffectedObject != null) objectRenderer = AffectedObject.GetComponent<Renderer>();
        if (objectRenderer != null) objectRenderer.material.color = newColor;
    }

    private void UpdateStatsUI()
    {
        if (StatsText != null)
        {
            // Vis den *gemte non-wheelchair* base speed, da det er "potentialet"
            float speedToShow = currentPlayerBaseSpeed;
            string speedLabel = $"Speed: {speedToShow:F1}";
            // Tilføj note hvis i kørestol
            if (wheelchairBound)
            {
                speedLabel += $" (Wheelchair: {wheelchairSpeed:F1})";
            }


            string gamblingStatus = gamblingMeterActive ? $"{gamblingMeterValue:F0}%" : "Inactive";

            StatsText.text =
                $"HP: {health}\n" +
                $"{speedLabel}\n" + // Opdateret speed label
                $"Blindness: {blindness:F0}%\n" +
                $"Wheelchair: {(wheelchairBound ? "Yes" : "No")}\n" +
                $"Stealth: {stealth:F0}%\n" +
                $"Gambling: {gamblingStatus}\n" +
                $"Energy Depletion: {energyDepletion:F0}%";
        }
    }
}