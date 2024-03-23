using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    public bool inDialougeMode;

    // Accessibles for the UI
    public TextMeshProUGUI dialougeText;
    public Canvas dialougeCanvas;
    public Text clickToContinueText;

    private void Start()
    {
        _instance = this;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        walkingSource = audioSources[0];
        walkingSource.clip = walkingSFX;
        normalSource = audioSources[1];
        inDialougeMode = false;

        DontDestroyOnLoad(gameObject);
    }

//---------------------------------------DIAOLOUGE FUNCTIONALITY---------------------------------------//

    public void InitDialouge(string[] texts)
    {
        inDialougeMode = true;
        dialougeCanvas.enabled = true;
        clickToContinueText.enabled = false;

        StartCoroutine(DisplayTextsSequentially(texts, 0));
    }

    private IEnumerator DisplayTextsSequentially(string[] texts, int index)
    {
        if (index >= texts.Length)
        {
            // All texts displayed, exit dialogue
            ExitDialouge();
            yield break;
        }
        
        dialougeText.text = texts[index];
        dialougeText.maxVisibleCharacters = 0;

        while (dialougeText.maxVisibleCharacters < texts[index].Length)
        {
            dialougeText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.05f); // Delay between each character
        }

        clickToContinueText.enabled = true;

        while (!Input.GetMouseButtonDown(0)) // Wait for mouse click
        {
            yield return null;
        }

        clickToContinueText.enabled = false;
        StartCoroutine(DisplayTextsSequentially(texts, index + 1)); // Move to next text
    }

    public void ExitDialouge()
    {
        inDialougeMode = false;
        dialougeCanvas.enabled = false;
    }


//-----------------------------------------SFX FUNCTIONALITY------------------------------------------//

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
