using UnityEngine;
using UnityEngine.Events;

public class NightTrigger : MonoBehaviour
{
    public static NightTrigger instance { get; private set; }
    public enum NightState { Night1, Night2, Night3, Night4, Night5, Night6, Night7 }
    NightState nightState;
    public UnityEvent<NightState> onNightChange;

    private bool isUsed = false;
    public NightState nextNight; // Sæt dette i Unity Inspector


    void Awake()
    {
        if (instance != null && this != instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            //isUsed = true; // Gør så triggeren kun virker én gang
            onNightChange.Invoke(nightState);
            nightState++;
            Debug.Log("Nat skiftet til: " + nightState);
            //gameObject.SetActive(false); // Deaktiver triggeren
        }
    }
}
