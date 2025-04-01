using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roam : IState
{
    NPC npc;
    public Roam(NPC npc)
    {
        this.npc = npc;
    }
    // Start is called before the first frame update

    public void Enter()
    {
        Debug.Log("NPC er nu i Idle-tilstand");
    }

    public void Update()
    {
        // Implementér opdateringslogik her
    }

    public void Exit()
    {
        Debug.Log("NPC forlader Idle-tilstand");
    }
}