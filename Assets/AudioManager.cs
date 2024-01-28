using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private bool isPlaying = false;

    public float playDuration = 10f; // Adjust the duration as needed

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
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();

            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            float deltaTime = 0f;
            while (deltaTime < audioSource.clip.length)
            {
                deltaTime += Time.deltaTime;
                yield return null;
            }

            timer += audioSource.clip.length;
        }

        isPlaying = false;
    }
}
