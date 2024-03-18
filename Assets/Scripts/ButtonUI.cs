using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene("PlayTestScene");
    }
}
