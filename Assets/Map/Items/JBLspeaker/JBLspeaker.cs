using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>(); // Finder AudioSource i et child-objekt
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop(); // Stopper sangen og nulstiller til start
                audioSource.Play(); // Starter sangen fra begyndelsen
            }
        }
    }
}