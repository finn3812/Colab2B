using UnityEngine;
using UnityEngine.UI;
using System;

public class SpinWheelManager
{
    private Image spinWheelImg;
    private SpinWheelImageSO spinWheelSO;
    private float currentSpeed = 0f;    // Nuvarande rotationshastighed
    private Action<float> onRotateUpdate; // Funktion der skal kaldes for at rotere hjulet visuelt
    private Action<int> onSpinComplete;   // Funktion der skal kaldes når hjulet stopper
    private bool isSpinning = false;    // Spinner hjulet lige nu?

    private const int SEGMENTS = 7; // Fast antal segmenter på hjulet

    public SpinWheelManager(Image wheelImage, SpinWheelImageSO spinWheelData, Action<float> rotateCallback, Action<int> spinCompleteCallback)
    {
        // Fejlhåndtering i constructor
        if (wheelImage == null) { Debug.LogError("SpinWheelManager: wheelImage parameter må ikke være null!"); return; }
        if (spinWheelData == null) { Debug.LogError("SpinWheelManager: spinWheelData parameter må ikke være null!"); return; }
        if (rotateCallback == null) { Debug.LogError("SpinWheelManager: rotateCallback parameter må ikke være null!"); return; }
        if (spinCompleteCallback == null) { Debug.LogError("SpinWheelManager: spinCompleteCallback parameter må ikke være null!"); return; }

        this.spinWheelImg = wheelImage;
        this.spinWheelSO = spinWheelData;
        this.onRotateUpdate = rotateCallback;
        this.onSpinComplete = spinCompleteCallback;

        UpdateWheelImage(); // Sæt hjulets grafik fra SO
    }

    // Opdaterer hjulets sprite fra Scriptable Object
    public void UpdateWheelImage()
    {
        if (spinWheelImg != null && spinWheelSO != null && spinWheelSO.SpinWheelImg != null)
        {
            spinWheelImg.sprite = spinWheelSO.SpinWheelImg;
            spinWheelImg.preserveAspect = true; // God idé at have på
        }
        else if (spinWheelImg != null && spinWheelSO != null && spinWheelSO.SpinWheelImg == null)
        {
            Debug.LogWarning("SpinWheelManager: Det tildelte SpinWheelImageSO har intet Sprite.", spinWheelSO);
        }
    }

    // Starter et spin
    public void StartSpin(float initialSpeed)
    {
        if (isSpinning)
        {
            Debug.LogWarning("SpinWheelManager: Forsøg på at starte spin mens det allerede kører.");
            return; // Start ikke et nyt spin oveni et eksisterende
        }

        isSpinning = true;
        currentSpeed = Mathf.Max(100f, initialSpeed); // Sørg for en minimum starthastighed
        Debug.Log($"SpinWheelManager: Starter spin med hastighed: {currentSpeed}");
    }

    // Skal kaldes fra Update() i den styrende klasse (SpinWheelController)
    public void UpdateSpin(float deltaTime)
    {
        if (!isSpinning) return; // Gør intet hvis hjulet ikke spinner

        // Sænk hastigheden over tid (exponentiel deceleration)
        if (currentSpeed > 1f) // Stop når hastigheden er meget lav
        {
            float rotationThisFrame = currentSpeed * deltaTime;
            onRotateUpdate?.Invoke(rotationThisFrame); // Kald callback for at rotere billedet
            currentSpeed *= 0.995f; // Brems gradvist (juster 0.995 for hurtigere/langsommere stop)
        }
        else
        {
            // Hjulet er stoppet
            currentSpeed = 0;
            isSpinning = false;
            Debug.Log("SpinWheelManager: Spin stoppet.");
            DetermineSegment(); // Find ud af hvilket segment det landede på
        }
    }

    // Beregner hvilket segment hjulet stoppede på baseret på rotationen
    private void DetermineSegment()
    {
        if (spinWheelImg == null)
        {
            Debug.LogError("SpinWheelManager: Kan ikke bestemme segment, spinWheelImg er null!");
            onSpinComplete?.Invoke(-1); // Kald med ugyldigt segment ved fejl
            return;
        }

        // Få Z rotation (0-360 grader)
        float rotationZ = spinWheelImg.rectTransform.eulerAngles.z;

        // Hjulet roterer MOD uret (negativ retning i Rotate).
        // 0 grader er typisk 'op'. Vi skal finde vinklen MED uret fra toppen.
        float clockwiseRotationFromTop = (360f - rotationZ) % 360f;

        // Beregn vinklen per segment
        float segmentAngle = 360f / (float)SEGMENTS; // Vigtigt med float division

        // Find segment index (0-baseret)
        int segment = Mathf.FloorToInt(clockwiseRotationFromTop / segmentAngle);

        // Korriger evt. for små unøjagtigheder nær 360/0 grader
        segment = segment % SEGMENTS;

        // Debug.Log($"Z rotation: {rotationZ:F2}, CW from Top: {clockwiseRotationFromTop:F2}, SegAngle: {segmentAngle:F2}, Calc Segment: {segment}");

        // Kald spin complete callback med det fundne segment index
        onSpinComplete?.Invoke(segment);
    }

    // Returnerer true hvis hjulet spinner lige nu
    public bool IsSpinning()
    {
        return isSpinning;
    }
}