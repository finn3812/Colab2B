using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class BuyStuff : MonoBehaviour
{
    private bool isShopOpen = false;
    public int ItemValue;
    public KeyCode BuyKey = KeyCode.E;
    public float Buyradius = 4f;
    public Transform Player;
    public GameObject Shop;
    public UnityEngine.UI.Button Flashbomb;
    public UnityEngine.UI.Button Rundstykker;
    public UnityEngine.UI.Button Batterier;
    public UnityEngine.UI.Button Exit;
    public Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        Currency.instance.Penge = 10;
        
        Shop.SetActive(false);
    }


  
    // Update is called once per frame
    void Update()
    {
        Currency.instance.GetMoney();
        float distance = Vector3.Distance(Player.position, transform.position);
        if (Input.GetKeyDown(BuyKey) && distance <= Buyradius)
        {
            isShopOpen = !isShopOpen;
          Shop.SetActive(true);
            if (isShopOpen)
            {
                if (playerMovement != null) playerMovement.enabled = false;
                UnityEngine.Cursor.lockState = CursorLockMode.None; UnityEngine.Cursor.visible = true;
            }
            else
            {
                if (playerMovement != null)
                {
                    playerMovement.enabled = true;
                    playerMovement.UpdateCursorState();
                }
                else { /* Fallback cursor lock */ UnityEngine.Cursor.lockState = CursorLockMode.Locked; UnityEngine.Cursor.visible = false; }
            }
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
        isShopOpen = false;
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.UpdateCursorState();
        }
        else { /* Fallback cursor lock */ UnityEngine.Cursor.lockState = CursorLockMode.Locked; UnityEngine.Cursor.visible = false; }
    }
}//Currency.instance.Penge >= ItemValue 


