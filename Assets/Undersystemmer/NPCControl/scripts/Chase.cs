using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : IState
{
    private readonly NavMeshAgent agent;
    NPC npc;
    public Chase(NPC npc)
    {
        this.npc = npc;
        agent = npc.agent;
    }
    // Start is called before the first frame update

    public void Enter()
    {
        agent.speed = npc.RunSpeed; // Use walk speed for roaming
        agent.isStopped = false; // Ensure agent can move
    }

    public void Update()
    {
        agent.SetDestination(npc.player.position);
        if (npc.AttackRange >= Vector3.Distance(npc.player.transform.position, npc.transform.position)) 
        {
            npc.TransitionToState(new Attack(npc));
        }

        if (!npc.CanSeePlayer())
        {
            npc.TransitionToState(new Suspicious(npc));
        }

        if (npc.name == "NPC test")
            AudioManager.Instance.PlaySound3D(AudioManager.Instance.finnChasingSound, npc.transform.position, 1f, 15f);

    }

    public void Exit()
    {
        Debug.Log("NPC forlader Idle-tilstand");
    }

}


