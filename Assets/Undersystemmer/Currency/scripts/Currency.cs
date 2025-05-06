using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public static Currency instance { get; private set; }
    public TMP_Text GuldMængde;
    public int Penge;
    public int MønteMængde;
    // Start is called before the first frame update


    private void Awake()

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


    void Start()
    {
        GuldMængde.text = Penge + " Guld";
    }

    public void SamlMønt()
    {
        MønteMængde = Random.Range(1, 17);
        Penge = Penge + MønteMængde;
        GetMoney();
        Debug.Log("Dødspengene er " + MønteMængde);
    }
    public void GetMoney()
    {
        GuldMængde.text = Penge + " Guld";
    }
    // Update is called once per frame

    public void KøbItem()
    {
      
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Penge = Penge + 2;
            GetMoney();
            Debug.Log("knaptryk er trykket");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SamlMønt();
            Debug.Log("knaptryk er trykket");
        }
    }
    
    

    
}
