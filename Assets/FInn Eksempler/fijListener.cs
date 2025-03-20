using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fijListener : MonoBehaviour
{
    fijEvent fijEvent;
    // Start is called before the first frame update
    void Start()
    {
        fijEvent = GameObject.Find("event").GetComponent<fijEvent>();
        fijEvent.keyPressed.AddListener(OnPrintEvent);
    }

    private void OnPrintEvent(KeyCode key)
    {
        Debug.Log(key.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
