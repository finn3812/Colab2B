using UnityEngine;
using UnityEngine.AI;

// This class implements the IState interface defined in NPC.cs
public class Idle : IState
{
    private readonly NPC npc; // Reference to the NPC owner
    private readonly NavMeshAgent agent; // Reference to the agent
    private float idleTimer; // How long we've been idling
    private float idleDuration; // How long we should idle for (chosen randomly)

    // Constructor
    public Idle(NPC npcController)
    {
        npc = npcController;
        agent = npc.agent;
    }

    public void Enter()
    {
        // Debug.Log("Entering Idle State");

        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath(); // Stop movement and clear path
        }

        // Set a random idle duration between 1 and 3 seconds
        idleDuration = Random.Range(1.0f, 3.0f);
        idleTimer = 0f;
    }

    public void Update()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            // Time's up, transition back to Roam state (if waypoints exist)
            if (npc.WayPoints != null && npc.WayPoints.Length > 0)
            {
                npc.TransitionToState(new Roam(npc));
            }
            else
            {
                // Stay Idle if no waypoints
                idleTimer = 0f; // Reset timer to loop idle state
            }
        }
        if (npc.CanSeePlayer())
        {
            npc.TransitionToState(new Chase(npc));
        }
    }

    public void Exit()
    {
         Debug.Log("Exiting Idle State");
    }
}