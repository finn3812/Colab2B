using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public IState currentState;
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
    SphereCollider SpCollider;




    private void Start()
    {
        SpCollider = transform.GetComponent<SphereCollider>();
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

    virtual public void Update()
    {
        currentState.Update();
        Debug.Log(currentState.ToString());
        if (currentState.ToString() == "Chase")
        {
            ViewAngle = 360;
        }
        else 
        {
            ViewAngle = 180;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpCollider.radius = 5;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpCollider.radius = 1;
        }
    }
    virtual public void TransitionToState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
   virtual public bool CanSeePlayer()
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
    virtual public bool CanHearPlayer()
    {
        SpCollider.radius = 5;
        return false;
    }
    virtual public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= AttackRange;
    }

}