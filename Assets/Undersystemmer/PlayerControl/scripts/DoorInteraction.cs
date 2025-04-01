using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorInteraction : MonoBehaviour
{
    public Transform doorTransform; // Assign the door's Transform in the inspector
    public float openAngle = 90f; // How far the door opens
    public float openSpeed = 2f; // Speed of door movement
    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = doorTransform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Toggle door state
            StopAllCoroutines();
            StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
        }
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
