using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class BuyStuff : MonoBehaviour
{
    public int ItemValue;
    public KeyCode BuyKey = KeyCode.E;
    public float Buyradius = 4f;
    public Transform Player;
    public GameObject Shop;
    public UnityEngine.UI.Button Flashbomb;
    public UnityEngine.UI.Button Rundstykker;
    public UnityEngine.UI.Button Batterier;
    public UnityEngine.UI.Button Exit;

    // Start is called before the first frame update
    void Start()
    {
        Currency.instance.Penge = 10;
    }


  
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        if (Input.GetKeyDown(BuyKey) && distance <= Buyradius)
        { 
          Shop.SetActive(true);
        }
    }

    //GEM TIL SENERE
    // Currency.instance.Penge -= ItemValue;
    //              Debug.Log("Ting Købt");




    //  Currency.instance.GetMoney();




    public void buyFlash()
    {
        ItemValue = 10;
        if (Currency.instance.Penge >= ItemValue)
        {
            Currency.instance.Penge -= ItemValue;
            Debug.Log("Ting Købt");
            Currency.instance.GetMoney();
            //indsæt del af item script til at få flashbomb her
        }
    }


    public void buyRundstyk()
    {
        ItemValue = 6;
        if (Currency.instance.Penge >= ItemValue)
        {
            Currency.instance.Penge -= ItemValue;
            Debug.Log("Ting Købt");
            Currency.instance.GetMoney();
            //indsæt del af item script til at få rundstykker her
        }
    }


    public void buyBatteri()
    {
        ItemValue = 5;
        if (Currency.instance.Penge >= ItemValue)
        {
            Currency.instance.Penge -= ItemValue;
            Debug.Log("Ting Købt");
            Currency.instance.GetMoney();
            //indsæt del af item script til at få batteri her
        }
    }
    public void ShopExit()
    {
        Shop.SetActive(false);
    }
}//Currency.instance.Penge >= ItemValue 


