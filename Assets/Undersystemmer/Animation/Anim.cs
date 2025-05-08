using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    Animator animator;
    public enum states
    {
        Idle,
        Roam,
        Chase,
        Attack,
        Jumpscare,
        Dead
    }
    public states currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case states.Idle:
                // Code for Idle state
                animator.SetInteger("State", 0);
                Debug.Log("The character is idle.");
                break;

            case states.Roam:
                // Code for Roam state
                animator.SetInteger("State", 1);
                Debug.Log("The character is roaming.");
                break;

            case states.Chase:
                // Code for Chase state
                animator.SetInteger("State", 2);
                Debug.Log("The character is chasing.");
                break;

            case states.Attack:
                // Code for Attack state
                animator.SetInteger("State", 3);
                Debug.Log("The character is attacking.");
                break;

            case states.Jumpscare:
                // Code for Jumpscare state
                animator.SetInteger("State", 4);
                Debug.Log("The character is jumpscaring.");
                break;

            case states.Dead:
                // Code for Dead state
                animator.SetInteger("State", 5);
                Debug.Log("The character is dead.");
                break;

            default:
                Debug.LogWarning("Unknown state.");
                break;
        }
    }
}
