using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public TMP_Text GuldM�ngde; 
    public int Penge;
    public int M�nteM�ngde;
    // Start is called before the first frame update
    void Start()
    {
        GuldM�ngde.text = Penge + " Guld";
    }

    void SamlM�nt()
    {
        M�nteM�ngde = Random.Range(1, 17);
        Debug.Log("D�dspengene er " + M�nteM�ngde);
    }
    void GetMoney()
    {
       GuldM�ngde.text = Penge + " Guld";
    }
    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.G))
    {
        Penge=Penge+2;
        GetMoney();
        Debug.Log("knaptryk er trykket");
    }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SamlM�nt();
            Penge = Penge + M�nteM�ngde;
            GetMoney();
            Debug.Log("knaptryk er trykket");
        }
    }
}
