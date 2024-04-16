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

    public AudioClip[] nonTalkingClips;
    public AudioClip[] perfumistClips;
    public AudioClip[] agnesClips;
    private AudioSource walkingSource;
    private AudioSource normalSource;

    public bool inDialougeMode;

    public int frequencyLevel;

    // Accessibles for the UI
    public TextMeshProUGUI dialougeText;
    public Canvas dialougeCanvas;
    public Text clickToContinueText;

    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        walkingSource = audioSources[0];
        walkingSource.clip = walkingSFX;
        normalSource = audioSources[1];
        inDialougeMode = false;

        DontDestroyOnLoad(gameObject);
    }

//---------------------------------------DIAOLOUGE FUNCTIONALITY---------------------------------------//

    public void InitDialouge(string[] texts, string dialougeType)
    {
        inDialougeMode = true;
        dialougeCanvas.enabled = true;
        clickToContinueText.enabled = false;

        StartCoroutine(DisplayTextsSequentially(texts, 0, dialougeType));
    }

    private IEnumerator DisplayTextsSequentially(string[] texts, int index, string dialougeType)
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
            PlayDialougeSound(dialougeText.maxVisibleCharacters, dialougeType);
            dialougeText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.05f); // Delay between each character
        }

        clickToContinueText.enabled = true;

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        clickToContinueText.enabled = false;
        StartCoroutine(DisplayTextsSequentially(texts, index + 1, dialougeType)); // Move to next text
    }

    public void ExitDialouge()
    {
        inDialougeMode = false;
        dialougeCanvas.enabled = false;
    }

    private void PlayDialougeSound(int currentDisplayedCharacterCount, string dialougeType)
    {
      if(currentDisplayedCharacterCount % frequencyLevel == 0)
      {

        AudioClip[] currTalkingClips;
        switch (dialougeType)
        {
            case "Agnes":
                currTalkingClips = perfumistClips; //   todo agnesClips;
                break;
            case "Perfumist":
                currTalkingClips = perfumistClips;
                break;
            default:
                currTalkingClips = nonTalkingClips;
                break;
        }

        AudioClip soundClip = null;
        int randomIndex = UnityEngine.Random.Range(0, currTalkingClips.Length);
        soundClip = currTalkingClips[randomIndex];

        normalSource.pitch = UnityEngine.Random.Range(0.9f, 1.2f);

        normalSource.PlayOneShot(soundClip);
      }
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
