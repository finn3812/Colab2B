using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public GameObject door;
    private bool timerActive = false;
    private float elapsedTime = 0f;

    public enum DayState { Day1, Night1, Day2, Night2, Day3, Night3, Day4, Night4, Day5, Night5, Day6, Night6, Day7, Night7 }
    public UnityEvent<DayState> onDayStateChange;

    private DayState currentDayState = DayState.Day1; // Starttilstand

    void Update()
    {
        if (timerActive)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("Tid inde i trigger: " + elapsedTime.ToString("F2") + " sekunder");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerActive = true;
            Debug.Log("Timer startet!");
            Debug.Log("Er kommet ind i triggeren");

            // Skift tidstilstand
            SwitchTimeState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerActive = false;
            Debug.Log("Er kommet ud af trigger");

            // Skift til næste fase (dag ? nat, nat ? dag)
            SwitchTimeState();
        }
    }

    private void SwitchTimeState()
    {
        if (currentDayState == DayState.Night7)
        {
            Debug.Log("Spillet er færdigt, ingen flere skift!");
            return;
        }

        // Skift til den næste dag/nat-tilstand
        currentDayState++;
        onDayStateChange.Invoke(currentDayState);
        Debug.Log("Tid skiftet til: " + currentDayState);
    }
}
