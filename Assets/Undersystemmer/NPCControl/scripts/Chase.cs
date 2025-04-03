using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    NPC npc;
    public Chase(NPC npc)
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
        if (npc.AttackRange >= Vector3.Distance(npc.player.transform.position, npc.transform.position)) 
        {
            npc.TransitionToState(new Attack(npc));
        }
    }

    public void Exit()
    {
        Debug.Log("NPC forlader Idle-tilstand");
    }

    void Start()
    {
        npc.TransitionToState(new Roam(npc));
    }
}


