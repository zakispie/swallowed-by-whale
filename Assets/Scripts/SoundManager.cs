using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance of the SoundManager
    private static SoundManager _instance;

    // Read-only instant of SoundManager to access from other scripts
    public static SoundManager Instance => _instance;

    public AudioClip walkingSFX;
    public AudioClip shootingSFX;
    public AudioClip powerupSFX;

    private AudioSource walkingSource;
    private AudioSource normalSource;

    private void Start()
    {
        _instance = this;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        walkingSource = audioSources[0];
        walkingSource.clip = walkingSFX;
        normalSource = audioSources[1];

        DontDestroyOnLoad(gameObject);
    }

    public void PlayWalkingSFX()
    {
        if (!walkingSource.isPlaying)
        {
            walkingSource.Play();
        }
    }

    public void StopWalkingSFX()
    {   
        walkingSource.Stop();
    }

    public void PlayShootingSFX()
    {
        normalSource.PlayOneShot(shootingSFX);
    }

    public void PlayPowerupSFX()
    {
        normalSource.PlayOneShot(powerupSFX);
    }
}
