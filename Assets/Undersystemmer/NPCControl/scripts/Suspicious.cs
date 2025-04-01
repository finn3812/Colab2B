using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Suspicious : MonoBehaviour
{ 
    
    NPC npc;
    public Suspicious(NPC npc)
    {
        this.npc = npc;
        
    }
    public void Enter() { }
    public void Update()
    {
        Debug.Log("Suspicious... Moving to last known position.");

        // Set destination to the last known position of the player
        npc.agent.SetDestination(npc.lastKnownPosition);


        if (npc.CanSeePlayer())
        {
            npc.lastKnownPosition = npc.player.position; // Update last known position
            npc.TransitionToState(new Chase(npc));
            Debug.Log("Player spotted again! Resuming hunt.");
        }

        // Check if we've reached the last known position
        if ((npc.agent.remainingDistance < 0.5f) && (!npc.CanSeePlayer()))
        {
            // If the player is still not visible, go back to patrol
            npc.TransitionToState(new Roam(npc));
            Debug.Log("Player lost. Returning to patrol.");

        }
    }

    public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(npc.transform.position, npc.player.position) <= npc.AttackRange;
    }
    public void Exit() { }
}
