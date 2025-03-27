using UnityEngine;
using UnityEngine.UI;

public class SpinWheelController : MonoBehaviour
{
    public Image SpinWheelImg;  // UI Image fra Canvas
    public SpinWheelImageSO SpinWheelData; // ScriptableObject med billede
    public Button SpinBtn; // Knap til at starte spin
    public RectTransform FingerSpinWheel; // Pilen der peger på hjulet

    private SpinWheelManager spinWheelManager;

    void Start()
    {
        spinWheelManager = new SpinWheelManager(SpinWheelImg, SpinWheelData, RotateWheel, HandleSegmentHit);
        SpinBtn.onClick.AddListener(StartSpin); // Når man trykker på knappen, starter spin
    }

    void Update()
    {
        spinWheelManager.UpdateSpin(Time.deltaTime); // Opdaterer rotation i Unity's Update
    }

    private void StartSpin()
    {
        spinWheelManager.StartSpin(5000f); // Starter spin med lavere hastighed (ændret fra 1000f)
    }

    private void RotateWheel(float rotationAmount)
    {
        SpinWheelImg.transform.Rotate(0, 0, rotationAmount); // Drejer hjulet
    }

    // Håndterer hvad der sker, når vi rammer et segment
    private void HandleSegmentHit(int segment)
    {
        Debug.Log("Segment Hit: " + segment);

        // Her kan du bruge `segment` til at vise ny ting
        // For eksempel, ændre teksten eller vise noget nyt baseret på segmentet
        // Hvis du har 7 felter, kan du tilknytte en ting til hver sektion:
        switch (segment)
        {
            case 0:
                // Skift til noget nyt
                Debug.Log("hej");
                break;
            case 1:
                // Skift til noget andet
                Debug.Log("hej");
                break;
            // ... fortsæt for de andre sektioner

            default:
                break;
        }
    }
}
