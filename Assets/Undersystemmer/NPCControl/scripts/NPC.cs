using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public interface IState
{
    public void Enter();
    public void Update();
    public void Exit();
}

public class NPC : MonoBehaviour
{
    public Transform player;
    private IState currentState;
    public float WalkSpeed;
    public float RunSpeed;
    public Transform[] WayPoints;
    public bool Night;
    // public Animation
    public int AttackRange;
    public int WaitTime;
    // public player.Sound = PlayerSound
    public float SoundMultiplier;
    public int ViewDistance;
    public int ViewAngle;
    public Transform PlaterPosition;
    public Vector3 lastKnownPosition;
    public NavMeshAgent agent;
    public int Health; 


    private void Start()
    {
        // Get the agent component FIRST
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NPC requires a NavMeshAgent component!", this);
            enabled = false; // Stop the script if no agent
            return;
        }

        // THEN transition to the initial state
        TransitionToState(new Idle(this));
    }

    public void Update()
    {
        currentState.Update();
    }
    public void TransitionToState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
    public bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > ViewDistance) return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > ViewAngle / 2) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, ViewDistance))
        {
            if (hit.collider.transform == player)
            {
                return true;
            }
        }
        return false;
        }
    public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= AttackRange;
    }

}