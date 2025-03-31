using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fijObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        singletomEx.instance.time.AddListener(testFunk);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void testFunk()
    {
        Debug.Log(gameObject.name);
    }

    private void OnDestroy()
    {
        singletomEx.instance.time.RemoveListener(testFunk);
    }
}
