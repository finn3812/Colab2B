using UnityEngine;
using UnityEngine.UI;
using TMPro; // Husk at bruge TextMeshPro

public class SpinWheelController : MonoBehaviour
{
    public Image SpinWheelImg;
    public SpinWheelImageSO SpinWheelData;
    public Button SpinBtn;
    public RectTransform FingerSpinWheel;
    public GameObject SpinCanvas;
    public GameObject AffectedObject;
    public TMP_Text StatsText; // UI-tekst til stats

    private SpinWheelManager spinWheelManager;
    private Renderer objectRenderer;

    // Spillerens stats
    private int health = 100;
    private float speed = 5f;
    private float blindness = 0f; // I procent
    private bool wheelchairBound = false;
    private float stealth = 0f; // I procent
    private bool gamblingMeter = false;
    private float energyDepletion = 0f; // I procent

    void Start()
    {
        spinWheelManager = new SpinWheelManager(SpinWheelImg, SpinWheelData, RotateWheel, HandleSegmentHit);
        SpinBtn.onClick.AddListener(StartSpin);

        SpinCanvas.SetActive(false); // Skjuler Canvas fra start

        if (AffectedObject != null)
            objectRenderer = AffectedObject.GetComponent<Renderer>();

        UpdateStatsUI(); // Viser start-stats
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleCanvas();
        }

        spinWheelManager.UpdateSpin(Time.deltaTime);
    }

    private void StartSpin()
    {
        spinWheelManager.StartSpin(3000f);
    }

    private void RotateWheel(float rotationAmount)
    {
        SpinWheelImg.transform.Rotate(0, 0, rotationAmount);
    }

    private void HandleSegmentHit(int segment)
    {
        if (AffectedObject == null) return;

        switch (segment)
        {
            case 0:
                health += 20;
                Debug.Log("Buff: Mere HP!");
                ChangeObjectColor(Color.red);
                break;
            case 1:
                wheelchairBound = true;
                speed = 0;
                Debug.Log("Curse: Wheelchair bound!");
                ChangeObjectColor(Color.yellow);
                break;
            case 2:
                speed += 2f;
                Debug.Log("Buff: Hurtigere bevægelse!");
                ChangeObjectColor(Color.yellow);
                break;
            case 3:
                gamblingMeter = true;
                Debug.Log("Curse: Gambling meter!");
                ChangeObjectColor(Color.green);
                break;
            case 4:
                stealth += 25f;
                if (stealth > 100f) stealth = 100f;
                Debug.Log("Buff: Stealth øget!");
                ChangeObjectColor(Color.blue);
                break;
            case 5:
                blindness += 50f;
                if (blindness > 100f) blindness = 100f;
                Debug.Log("Curse: Blindness øget!");
                ChangeObjectColor(new Color(0.5f, 0f, 0.5f));
                break;
            case 6:
                energyDepletion += 30f;
                if (energyDepletion > 100f) energyDepletion = 100f;
                Debug.Log("Curse: Energy depletion øget!");
                ChangeObjectColor(Color.magenta);
                break;
        }

        UpdateStatsUI(); // Opdater stats i UI
    }

    private void ChangeObjectColor(Color newColor)
    {
        if (objectRenderer == null)
            objectRenderer = AffectedObject.GetComponent<Renderer>();

        if (objectRenderer != null)
            objectRenderer.material.color = newColor;
    }

    private void UpdateStatsUI()
    {
        if (StatsText != null)
        {
            StatsText.text = $"HP: {health}\n" +
                             $"Speed: {speed}\n" +
                             $"Blindness: {blindness}%\n" +
                             $"Wheelchair Bound: {(wheelchairBound ? "Yes" : "No")}\n" +
                             $"Stealth: {stealth}%\n" +
                             $"Gambling Meter: {(gamblingMeter ? "Yes" : "No")}\n" +
                             $"Energy Depletion: {energyDepletion}%";
        }
    }

    private void ToggleCanvas()
    {
        SpinCanvas.SetActive(!SpinCanvas.activeSelf);
    }
}
