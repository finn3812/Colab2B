using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public TMP_Text GuldMængde; 
    public int Penge;
    public int MønteMængde;
    // Start is called before the first frame update
    void Start()
    {
        GuldMængde.text = Penge + " Guld";
    }

    void SamlMønt()
    {
        MønteMængde = Random.Range(1, 17);
        Debug.Log("Dødspengene er " + MønteMængde);
    }
    void GetMoney()
    {
       GuldMængde.text = Penge + " Guld";
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
            SamlMønt();
            Penge = Penge + MønteMængde;
            GetMoney();
            Debug.Log("knaptryk er trykket");
        }
    }
}
