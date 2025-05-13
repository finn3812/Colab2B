using UnityEngine;
using UnityEngine.AI;

// This class implements the IState interface defined in NPC.cs
public class Roam : IState
{
    private readonly NPC npc; // Reference to the NPC owner of this state
    private readonly NavMeshAgent agent; // Reference to the NPC's agent
    private readonly Transform[] waypoints; // Reference to the NPC's waypoints

    private int currentWaypointIndex = -1; // Keep track of the last destination index

    // Constructor to get references from the NPC
    public Roam(NPC npcController)
    {
        npc = npcController;
        agent = npc.agent;
        waypoints = npc.WayPoints;
    }

    public void Enter()
    {
        // Debug.Log("Entering Roam State");
        agent.speed = npc.WalkSpeed; // Use walk speed for roaming
        agent.isStopped = false; // Ensure agent can move

        // --- Choose a new random waypoint ---
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("NPC has no waypoints defined. Cannot Roam. Switching to Idle.", npc.gameObject);
            npc.TransitionToState(new Idle(npc));
            return;
        }

        int previousWaypointIndex = currentWaypointIndex;
        if (waypoints.Length > 1)
        {
            while (currentWaypointIndex == previousWaypointIndex)
            {
                currentWaypointIndex = Random.Range(0, waypoints.Length);
            }
        }
        else { currentWaypointIndex = 0; } // Only one waypoint

        // --- Set Destination ---
        Transform destinationWaypoint = waypoints[currentWaypointIndex];
        if (destinationWaypoint != null)
        {
            agent.SetDestination(destinationWaypoint.position);
        }
        else
        {
            Debug.LogError($"Waypoint at index {currentWaypointIndex} is null! Switching to Idle.", npc.gameObject);
            npc.TransitionToState(new Idle(npc));
        }
    }

    public void Update()
    {
        // --- Check if player is visible FIRST (higher priority) ---
        if (npc.CanSeePlayer())
        {
            npc.TransitionToState(new Chase(npc));
            return; // Exit Update early after transitioning
        }


        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Ensure agent has either no path or has stopped moving at the destination
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Reached the waypoint, transition to Idle state
                npc.TransitionToState(new Idle(npc));
            }
        }
    }

    public void Exit()
    {
         //Debug.Log("Exiting Roam State");
    }
}