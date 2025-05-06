using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuyStuff : MonoBehaviour
{
    public int ItemValue;
    public KeyCode BuyKey = KeyCode.E;
    public float Buyradius = 4f;
    public Transform Player;
    public GameObject Shop;

    // Start is called before the first frame update
    void Start()
    {
        Currency.instance.Penge = 10;
    }


  
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        if (Currency.instance.Penge >= ItemValue && Input.GetKeyDown(BuyKey) && distance <= Buyradius)
        { 
          Shop.SetActive(true);
        }
    }

    //GEM TIL SENERE
   // Currency.instance.Penge -= ItemValue;
  //              Debug.Log("Ting Købt");
          //  Currency.instance.GetMoney();
}
