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
    }
}
