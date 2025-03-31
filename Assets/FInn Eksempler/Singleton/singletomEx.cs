using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class singletomEx : MonoBehaviour
{
    public static singletomEx instance { get; private set; }
    public UnityEvent time;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance!=null && this!=instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        InvokeRepeating("now", 1, 1);
    }

    void now()
    {
        time.Invoke();
    }

}
