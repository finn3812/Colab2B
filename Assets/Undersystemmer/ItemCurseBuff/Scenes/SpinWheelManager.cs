using UnityEngine;
using UnityEngine.UI;
using System;

public class SpinWheelManager
{
    private Image spinWheelImg;  // Reference til UI Image (hjulet)
    private SpinWheelImageSO spinWheelSO;  // Reference til ScriptableObject
    private float currentSpeed = 0f; // Hastighed på rotation
    private Action<float> onRotateUpdate; // Callback til rotation

    private Action<int> onSegmentHit; // Callback til at håndtere segmentet (ny ting)

    // Konstruktor
    public SpinWheelManager(Image wheelImage, SpinWheelImageSO spinWheelImage, Action<float> rotateCallback, Action<int> segmentCallback)
    {
        this.spinWheelImg = wheelImage;
        this.spinWheelSO = spinWheelImage;
        this.onRotateUpdate = rotateCallback;
        this.onSegmentHit = segmentCallback;

        UpdateWheelImage();
    }

    // Opdaterer billedet fra ScriptableObject
    public void UpdateWheelImage()
    {
        if (spinWheelImg != null && spinWheelSO != null)
        {
            spinWheelImg.sprite = spinWheelSO.SpinWheelImg;
        }
    }

    // Starter hjulets rotation med en given hastighed
    public void StartSpin(float initialSpeed)
    {
        currentSpeed = initialSpeed;
    }

    // Opdaterer rotationen (kaldes fra MonoBehaviour Update)
    public void UpdateSpin(float deltaTime)
    {
        if (currentSpeed > 0.5f) // Stopper kun hvis den er meget langsom
        {
            onRotateUpdate?.Invoke(currentSpeed * deltaTime); // Kald rotation callback
            currentSpeed *= 0.999f; // Langsommere opbremsning (fra 0.99f til 0.995f)

            // Beregn hvilken sektion vi er i
            int section = GetCurrentSection();
            onSegmentHit?.Invoke(section); // Kald segment callback
        }
        else
        {
            currentSpeed = 0;
        }
    }

    // Beregn hvilken sektion af hjulet vi er i (0-6, svarende til 7 sektioner)
    private int GetCurrentSection()
    {
        // Hent rotationen på Z-aksen
        float zRotation = spinWheelImg.transform.rotation.eulerAngles.z;

        // Find hvilket segment af de 7 vi er i
        int segment = Mathf.FloorToInt(zRotation / (360f / 7f)) % 7;
        return segment;
    }
}
