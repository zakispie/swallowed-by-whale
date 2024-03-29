using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    private static GameManager _instance;

    // Read-only instant of GameManager to access from other scripts
    public static GameManager Instance => _instance;

    // If a # exists in array, that level has been unlocked
    // 0 = Start Menu
    private int[] unlockedLevels = {0, 1};

    // The current level the player is on
    public int currLevel;

    void Start()
    {

        if (_instance == null)
        {
            currLevel = 0;
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SetLevel(int level)
    {
        _instance.currLevel = level;  
    }

}
