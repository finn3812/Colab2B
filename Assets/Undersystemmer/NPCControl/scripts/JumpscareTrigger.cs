using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))] // Recommend adding an AudioSource to the NPC itself
public class JumpscareTrigger : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign the Player's GameObject or a core Transform from it.")]
    public Transform playerTransform;

    [Tooltip("Assign the Canvas GameObject that contains the jumpscare image.")]
    public GameObject jumpscareCanvas; // Use GameObject to easily activate/deactivate

    [Tooltip("Assign the specific sound clip to play for the jumpscare.")]
    public AudioClip jumpscareSoundClip;

    // We'll get the AudioSource from this GameObject automatically
    private AudioSource jumpscareAudioSource;

    [Header("Settings")]
    [Tooltip("The distance threshold at which the jumpscare triggers.")]
    public float triggerDistance = 2.0f; // Adjusted default slightly

    [Tooltip("How long (in seconds) the jumpscare image stays visible.")]
    public float jumpscareDuration = 991.5f;

    [Tooltip("Should the jumpscare only happen once?")]
    public bool triggerOnce = true;

    // Internal state
    private bool hasTriggered = false;
    private Coroutine activeJumpscareCoroutine = null;
    private Transform monsterTransform; // Store our own transform

    void Start()
    {
        // Get the transform of this NPC GameObject
        monsterTransform = transform;

        // Get the AudioSource component attached to this same GameObject
        jumpscareAudioSource = GetComponent<AudioSource>();
        if (jumpscareAudioSource == null)
        {
            Debug.LogError("JumpscareTrigger: Requires an AudioSource component on the same GameObject!", this);
            enabled = false; // Disable script if no AudioSource found
            return;
        }
        jumpscareAudioSource.playOnAwake = false; // Ensure it doesn't play automatically

        // --- Reference Checks ---
        if (playerTransform == null)
        {
            Debug.LogError("JumpscareTrigger: Player Transform is not assigned!", this);
        }
        if (jumpscareCanvas == null)
        {
            Debug.LogError("JumpscareTrigger: Jumpscare Canvas is not assigned!", this);
        }
        if (jumpscareSoundClip == null)
        {
            Debug.LogError("JumpscareTrigger: Jumpscare Sound Clip is not assigned!", this);
        }

        // Ensure the jumpscare canvas is initially inactive
        if (jumpscareCanvas != null)
        {
            jumpscareCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Conditions to check before proceeding:
        if ((!hasTriggered || !triggerOnce) &&
            playerTransform != null && // Make sure player is assigned
            monsterTransform != null && // We have our own transform
            jumpscareCanvas != null &&
            jumpscareAudioSource != null &&
            jumpscareSoundClip != null &&
            activeJumpscareCoroutine == null)
        {
            // Calculate the distance between the player and THIS monster
            float distance = Vector3.Distance(playerTransform.position, monsterTransform.position);

            // Check if the player is within the trigger distance
            if (distance <= triggerDistance)
            {
                TriggerJumpscare();
            }
        }
    }

    void TriggerJumpscare()
    {
        if (hasTriggered && triggerOnce) return; // Extra safety check

        Debug.Log("Jumpscare Triggered by: " + gameObject.name); // Log which NPC triggered
        hasTriggered = true;

        // Activate the canvas
        jumpscareCanvas.SetActive(true);

        // Play the sound using the AudioSource on this NPC
        jumpscareAudioSource.clip = jumpscareSoundClip;
        jumpscareAudioSource.Play();

        // Start a coroutine to deactivate the jumpscare after a duration
        activeJumpscareCoroutine = StartCoroutine(DeactivateJumpscareAfterDelay(jumpscareDuration));
    }

    IEnumerator DeactivateJumpscareAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (jumpscareCanvas != null)
        {
            jumpscareCanvas.SetActive(false);
        }
        // Optional: Stop audio
        // if (jumpscareAudioSource != null) jumpscareAudioSource.Stop();

        Debug.Log("Jumpscare Finished by: " + gameObject.name);
        activeJumpscareCoroutine = null;
    }

    // Gizmo to visualize the trigger distance around THIS NPC
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerDistance); // Use this object's position
    }
}