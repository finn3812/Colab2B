using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinWheelController : MonoBehaviour
{
    public Image SpinWheelImg;
    public SpinWheelImageSO SpinWheelData;
    public Button SpinBtn;
    public RectTransform FingerSpinWheel;
    public GameObject SpinCanvas;
    public GameObject AffectedObject;
    public TMP_Text StatsText;
    public Slider GamblingMeterUI; // UI Slider til Gambling Meter

    private SpinWheelManager spinWheelManager;
    private Renderer objectRenderer;

    private int health = 100;
    private float speed = 5f;
    private float blindness = 0f;
    private bool wheelchairBound = false;
    private float stealth = 0f;
    private bool gamblingMeter = false;
    private float energyDepletion = 0f;
    private float gamblingMeterValue = 0f; // Starter på 0%

    private float gamblingIncreaseRate = 5f; // Hvor hurtigt den stiger per sekund

    void Start()
    {
        spinWheelManager = new SpinWheelManager(SpinWheelImg, SpinWheelData, RotateWheel, HandleSegmentHit);
        SpinBtn.onClick.AddListener(StartSpin);

        SpinCanvas.SetActive(false);

        if (AffectedObject != null)
            objectRenderer = AffectedObject.GetComponent<Renderer>();

        if (GamblingMeterUI != null)
        {
            GamblingMeterUI.value = gamblingMeterValue;
            GamblingMeterUI.gameObject.SetActive(false); // Skjuler slideren fra start
        }

        UpdateStatsUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleCanvas();
        }

        spinWheelManager.UpdateSpin(Time.deltaTime);

        // Gambling meter stiger over tid, hvis den er aktiveret
        if (gamblingMeter && GamblingMeterUI.gameObject.activeSelf)
        {
            gamblingMeterValue += gamblingIncreaseRate * Time.deltaTime;
            gamblingMeterValue = Mathf.Clamp(gamblingMeterValue, 0, 1000);

            if (GamblingMeterUI != null)
                GamblingMeterUI.value = gamblingMeterValue;
        }
    }

    private void StartSpin()
    {
        spinWheelManager.StartSpin(3000f);
        ResetGamblingMeter(); // Resetter gambling meter ved spin
    }

    private void RotateWheel(float rotationAmount)
    {
        if (SpinWheelImg != null)
        {
            SpinWheelImg.rectTransform.Rotate(0, 0, -rotationAmount); // Roterer mod uret
        }
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
            case 3: // Gambling Meter-segment
                if (!gamblingMeter) // Kun hvis spilleren ikke har fået den før
                {
                    gamblingMeter = true;
                    if (GamblingMeterUI != null)
                        GamblingMeterUI.gameObject.SetActive(true); // Gør slideren synlig første gang
                }
                Debug.Log("Curse: Gambling meter!");
                ChangeObjectColor(Color.green);
                break;
            case 4:
                stealth += 25f;
                stealth = Mathf.Clamp(stealth, 0, 100);
                Debug.Log("Buff: Stealth øget!");
                ChangeObjectColor(Color.blue);
                break;
            case 5:
                blindness += 50f;
                blindness = Mathf.Clamp(blindness, 0, 100);
                Debug.Log("Curse: Blindness øget!");
                ChangeObjectColor(new Color(0.5f, 0f, 0.5f));
                break;
            case 6:
                energyDepletion += 30f;
                energyDepletion = Mathf.Clamp(energyDepletion, 0, 100);
                Debug.Log("Curse: Energy depletion øget!");
                ChangeObjectColor(Color.magenta);
                break;
        }

        UpdateStatsUI();
    }

    private void ResetGamblingMeter()
    {
        if (gamblingMeter)
        {
            gamblingMeterValue = 0;
            if (GamblingMeterUI != null)
                GamblingMeterUI.value = gamblingMeterValue;
        }
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
                             $"Gambling Meter: {gamblingMeterValue}%\n" +
                             $"Energy Depletion: {energyDepletion}%";
        }
    }

    private void ToggleCanvas()
    {
        SpinCanvas.SetActive(!SpinCanvas.activeSelf);
    }
}
