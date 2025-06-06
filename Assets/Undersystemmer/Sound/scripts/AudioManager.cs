using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Baggrundsmusik")]
    public AudioSource musicSource;
    public AudioClip dayMusic;
    public AudioClip nightMusic;
    public float crossfadeDuration = 2f;

    [Header("Lydkilder")]
    public AudioSource sfxSourcePrefab;
    private List<AudioSource> active3DSounds = new List<AudioSource>();

    [Header("Roaming")]
    public AudioClip finnRoamingSound; 
    public AudioClip BaseRoamingSound; 
    public AudioClip BoRoamingSound;
    public AudioClip HansRoamingSound;
    

    [Header("Chasing")]
    public AudioClip finnChasingSound; 
    public AudioClip BaseChasingSound;
    public AudioClip BoChasingSound;
    public AudioClip HansChasingSound;
    public AudioClip KantinedameChasingSound;

    [Header("Attack")]
    public AudioClip finnAttackSound; 
    public AudioClip BaseAttackSound; 
    public AudioClip BoAttackSound;
    public AudioClip HansAttackSound;
    public AudioClip KantinedameAttackSound;

    [Header("Dead")]
    public AudioClip finnDeadSound; 
    public AudioClip BaseDeadSound; 
    public AudioClip BoDeadSound;
    public AudioClip HansDeadSound;
    public AudioClip KantinedameDeadSound;



    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(UpdateBackgroundMusic());
    }

    private IEnumerator UpdateBackgroundMusic()
    {
        while (true)
        {
            float timeOfDay = Time.time % 60;
            AudioClip newClip = timeOfDay < 30 ? dayMusic : nightMusic;

            if (musicSource.clip != newClip)
            {
                StartCoroutine(FadeMusic(newClip));
            }

            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = musicSource.volume;
        for (float t = 0; t < crossfadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / crossfadeDuration);
            yield return null;
        }

        musicSource.clip = newClip;
        musicSource.Play();

        for (float t = 0; t < crossfadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / crossfadeDuration);
            yield return null;
        }
    }

    public void PlaySound2D(AudioClip clip, float volume = 1f)
    {
        musicSource.PlayOneShot(clip, volume);
    }

    public void PlaySound3D(AudioClip clip, Vector3 position, float volume = 1f, float maxDistance = 10f)
    {
        AudioSource newSource = Instantiate(sfxSourcePrefab, position, Quaternion.identity);
        newSource.clip = clip;
        newSource.spatialBlend = 1f;
        newSource.volume = volume;
        newSource.maxDistance = maxDistance;
        newSource.rolloffMode = AudioRolloffMode.Linear;
        newSource.Play();
        Destroy(newSource.gameObject, clip.length);

        active3DSounds.Add(newSource);
    }

    public void PlayFinnRoamingSound(Vector3 position)
    {
        PlaySound3D(finnRoamingSound, position, 0.8f, 15f);
    }

    public void StopAll3DSounds()
    {
        foreach (var source in active3DSounds)
        {
            if (source != null) Destroy(source.gameObject);
        }
        active3DSounds.Clear();
    }
}
