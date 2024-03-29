using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Play button appears when a level is selected
    public GameObject playBtn;


    private int level;

    public void SelectLevel(int level)
    {
        this.level = level;
        GameManager.Instance.SetLevel(level);

        playBtn.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(level);
    }

}
