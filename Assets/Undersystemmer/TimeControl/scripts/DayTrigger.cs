using UnityEngine;
using UnityEngine.Events;

public class DayTrigger : MonoBehaviour
{
    public static DayTrigger instance { get; private set; }
    public enum DayState { Day1, Day2, Day3, Day4, Day5, Day6, Day7,Finish }
    static DayState dayState;
    static public UnityEvent<DayState> onDayChange;

    static private bool isUsed = false;
    public DayState nextDay; // S�t dette i Unity Inspector


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

    public static void isDay()
    {

        onDayChange.Invoke(dayState);
        dayState++;
        Debug.Log("Dag skiftet til: " + dayState);
        if (dayState == DayState.Finish)
        {
            isUsed = true;
        }
    }


/*    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            //isUsed = true; // G�r s� triggeren kun virker �n gang
            onDayChange.Invoke(dayState);
            dayState++;
            Debug.Log("Dag skiftet til: " + dayState);
            if (dayState == DayState.Finish)
            {
                isUsed = true;
            }
            //gameObject.SetActive(false); // Deaktiver triggeren
        }
    }*/
}
