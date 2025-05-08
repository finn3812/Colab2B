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
    public Movement movementScript;


    public void Start()
    {
        movementScript = player.GetComponent<Movement>();
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
        CanHearPlayer();
        
        Debug.Log(movementScript.IsSprint);

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
    virtual public void CanHearPlayer()
    {
        if (movementScript != null && movementScript.IsSprint)
        {
            SpCollider.radius = 100;
            Debug.Log("Can hear player, radius set to 100");
        }
        else if (movementScript.IsSprint ==false)
        {
            SpCollider.radius = 0;
            Debug.Log("Cannot hear player, radius set to 0");
        }
    }


    virtual public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= AttackRange;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone!");

            
            if (!(currentState is Chase))
            {
                TransitionToState(new Chase(this));
            }
        }
    }
        public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone.");

            if (currentState is Chase)
            {
                TransitionToState(new Suspicious(this));
            }
        }
    }
}