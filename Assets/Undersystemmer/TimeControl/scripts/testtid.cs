using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtid : MonoBehaviour
{
    DayTrigger time;
    // Start is called before the first frame update
    void Start()
    {
        //DayTrigger.instance.onDayChange.AddListener(testDayTrigger);
        time = DayTrigger.instance;

        NightTrigger.instance.onNightChange.AddListener(testNightTrigger);
    }
    
    void testDayTrigger(DayTrigger.DayState state)
    {
        Debug.Log(state);
    }

    void testNightTrigger(NightTrigger.NightState state)
    {
        Debug.Log(state);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DayTrigger.instance.isDay();
        }
    }
}


