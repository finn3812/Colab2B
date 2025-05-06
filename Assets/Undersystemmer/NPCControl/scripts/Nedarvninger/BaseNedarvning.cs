using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNedarvning : NPC
{
    override public void Update()
    {
        currentState.Update();
        Debug.Log(currentState.ToString()); 
        ViewAngle = 360;
        ViewDistance = 5; // ik ret langt han er vant til se en sk�rm der ikke er ret langt v�k
    }
    public override bool CanHearPlayer()
    {
        return false;
    }
}
