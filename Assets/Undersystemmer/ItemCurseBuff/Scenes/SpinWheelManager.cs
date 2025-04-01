using UnityEngine;
using UnityEngine.UI;
using System;

public class SpinWheelManager
{
    private Image spinWheelImg;
    private SpinWheelImageSO spinWheelSO;
    private float currentSpeed = 0f;
    private Action<float> onRotateUpdate;
    private Action<int> onSpinComplete;
    private bool isSpinning = false;

    private const int SEGMENTS = 7; // Antal segmenter

    public SpinWheelManager(Image wheelImage, SpinWheelImageSO spinWheelImage, Action<float> rotateCallback, Action<int> spinCompleteCallback)
    {
        this.spinWheelImg = wheelImage;
        this.spinWheelSO = spinWheelImage;
        this.onRotateUpdate = rotateCallback;
        this.onSpinComplete = spinCompleteCallback;

        UpdateWheelImage();
    }

    public void UpdateWheelImage()
    {
        if (spinWheelImg != null && spinWheelSO != null)
        {
            spinWheelImg.sprite = spinWheelSO.SpinWheelImg;
        }
    }

    public void StartSpin(float initialSpeed)
    {
        if (isSpinning) return; // Hvis hjulet allerede spinner, gør ingenting

        isSpinning = true;
        currentSpeed = initialSpeed;
    }

    public void UpdateSpin(float deltaTime)
    {
        if (!isSpinning) return; // Kun opdater, hvis hjulet spinner

        if (currentSpeed > 0.1f)
        {
            onRotateUpdate?.Invoke(currentSpeed * deltaTime);
            currentSpeed *= 0.998f; // Bremser langsomt
        }
        else
        {
            currentSpeed = 0;
            isSpinning = false;
            DetermineSegment(); // Kalder kun segment-funktionen når hjulet stopper
        }
    }

    private void DetermineSegment()
    {
        float rotationZ = spinWheelImg.transform.eulerAngles.z;
        int segment = Mathf.FloorToInt((rotationZ % 360) / (360 / SEGMENTS)); // Finder hvilket segment hjulet lander på
        onSpinComplete?.Invoke(segment); // Kalder funktionen, når hjulet stopper
    }
}
