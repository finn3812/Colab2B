using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement3D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private Rigidbody rb;

    private Vector3 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Bevægelsesretning i verden
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Opdater animatorens "Speed" parameter
        animator.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        // Flyt spilleren
        Vector3 movePosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(movePosition);

        // Roter spilleren i bevægelsesretningen
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }
}

// Start is called before the first frame update

