using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCoins : MonoBehaviour
{
    public Transform player; // Reffere til spiller
    public float Interactionradius = 2f; //Hvor man vil kunne samle pengene op fra.
    public KeyCode interactionKey = KeyCode.E; //knappen man skal trykke for at f� penge
    public float holdTime = 3f; //hvor lang tid knappen skal holdes

    private float holdTimer = 0f; //en timer til hvor lang tid knappen er blevet holdet nede
    private bool InRadius = false; // �siger efter om spilleren er inde for interactionsradiusen
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position); //distance fra objektet til spilleren

        if (distance <= Interactionradius) //tjekker efter om spilleren er inde for interaktionsradius
        {
            InRadius = true;
        }
        else
        {
            InRadius=false;
            holdTimer = 0f;
        }

        if(InRadius) 
        {
            if (Input.GetKeyDown(interactionKey)) //tjekker efter om spilleren trykker p� knappen
            {
                Debug.Log("Knappen holdes");

                holdTimer = Time.time; // begynder at t�lle med timeren
            }

            if (Input.GetKey(interactionKey))
            {

                if (Time.time - holdTimer >= holdTime) //tjekker efter om spilleren har hold nede i lang nok tid
                {
                    Currency.instance.SamlM�nt(); //giver random m�ngde penge og �ndre det p� UI
                    Destroy(this.gameObject); //�del�gger objektet
                    holdTimer = 0f;
                    InRadius = false;
                }

            }
       
              if(Input.GetKeyUp(interactionKey)) //reseter timeren hvis spilleren giver slip for tidlig.
            {
                holdTimer = 0f;
            }
                
               
            
        
        
        
        }
        else
        {
            holdTimer = 0f;
        }






    }
    
    


       

    
}
