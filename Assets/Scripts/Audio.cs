using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private bool isPlaying = false;

    public float playDuration = 10f; // Adjust the total duration as needed
    public float pitchMultiplier = 1.2f; // Adjust the pitch multiplier to speed up the audio clips

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            StartCoroutine(PlayRandomizedAudioCoroutine(playDuration));
        }
    }

    IEnumerator PlayRandomizedAudioCoroutine(float duration)
    {
        isPlaying = true;
        float timer = 0f;

        while (timer < duration)
        {
            int randomIndex = Random.Range(0, audioClips.Length - 1);
            audioSource.clip = audioClips[randomIndex];
            
            // Adjust pitch to speed up the audio clip
            audioSource.pitch = pitchMultiplier;

            audioSource.Play();

            yield return new WaitForSecondsRealtime(audioSource.clip.length / pitchMultiplier);

            timer += audioSource.clip.length / pitchMultiplier;
        }

        // Reset pitch to normal after the sequence is finished
        audioSource.pitch = 1f;

        isPlaying = false;
    }
}