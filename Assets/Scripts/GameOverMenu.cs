using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    public void RestartLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.currLevel);
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.currLevel = 0;
        SceneManager.LoadScene(0);
    }
}
