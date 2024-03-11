using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance of the SoundManager
    private static SoundManager _instance;

    // Read-only instant of SoundManager to access from other scripts
    public static SoundManager Instance => _instance;

    public AudioSource audioSource;

    public AudioClip walkingSFX;
    public AudioClip shootingSFX;
    public AudioClip powerupSFX;

    private void Start()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayWalkingSFX()
    {
        if (audioSource.clip != walkingSFX || !audioSource.isPlaying)
        {
            audioSource.clip = walkingSFX;
            audioSource.Play();
        }
    }

    public void PlayShootingSFX()
    {
        audioSource.PlayOneShot(shootingSFX);
    }

    public void PlayPowerupSFX()
    {
        audioSource.PlayOneShot(powerupSFX);
    }
}
