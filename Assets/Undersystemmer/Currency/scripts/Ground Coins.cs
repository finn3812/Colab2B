using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCoins : MonoBehaviour
{
    public Transform player; // Reffere til spiller
    public float Interactionradius = 2f; //Hvor man vil kunne samle pengene op fra.
    public KeyCode interactionKey = KeyCode.E; //knappen man skal trykke for at f� penge
   
  
    
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
            if (Input.GetKeyDown(interactionKey))
            {
                Currency.instance.SamlM�nt(); //giver random m�ngde penge og �ndre det p� UI
                Destroy(this.gameObject); //�del�gger objektet
            }
        }
        

        
           

            




    }
    
    


       

    
}
