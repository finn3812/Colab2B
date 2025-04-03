using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Transform doorTransform; // Assign the door's Transform in the Inspector
    public float openAngle = 90f; // Angle the door opens
    public float openSpeed = 2f; // Speed of opening/closing
    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Transform player;

    void Start()
    {
        closedRotation = doorTransform.rotation;
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Toggle state
            StopAllCoroutines();

            if (isOpen)
            {
                // Open away from the player
                float direction = DetermineDoorDirection();
                openRotation = closedRotation * Quaternion.Euler(0, openAngle * direction, 0);
                StartCoroutine(RotateDoor(openRotation));
            }
            else
            {
                // Close the door
                StartCoroutine(RotateDoor(closedRotation));
            }
        }
    }

    private float DetermineDoorDirection()
    {
        if (player == null) return 1f; // Default direction

        Vector3 playerDirection = player.position - doorTransform.position;
        float dot = Vector3.Dot(doorTransform.right, playerDirection);

        return dot > 0 ? -1f : 1f; // If player is on the right, open left; if on the left, open right
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float time = 0;
        Quaternion startRotation = doorTransform.rotation;

        while (time < 1)
        {
            time += Time.deltaTime * openSpeed;
            doorTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null;
        }
    }
}
