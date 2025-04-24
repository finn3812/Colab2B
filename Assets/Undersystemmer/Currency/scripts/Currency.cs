using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public static Currency instance { get; private set; }
    public TMP_Text GuldM�ngde;
    public int Penge;
    public int M�nteM�ngde;
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
        GuldM�ngde.text = Penge + " Guld";
    }

    public void SamlM�nt()
    {
        M�nteM�ngde = Random.Range(1, 17);
        Penge = Penge + M�nteM�ngde;
        GetMoney();
        Debug.Log("D�dspengene er " + M�nteM�ngde);
    }
    public void GetMoney()
    {
        GuldM�ngde.text = Penge + " Guld";
    }
    // Update is called once per frame

    public void K�bItem()
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
            SamlM�nt();
            Debug.Log("knaptryk er trykket");
        }
    }
    
    

    
}
