using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class fijEvent : MonoBehaviour
{
    public UnityEvent<KeyCode> keyPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            keyPressed.Invoke(KeyCode.A);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            keyPressed.Invoke(KeyCode.B);
        }
    }
}
