using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI ellipsisText;

    public void Awake()
    {
        ellipsisText.text = "...";
        StartCoroutine(DisplayEllipsis());
    }

    private IEnumerator DisplayEllipsis()
    {

        ellipsisText.maxVisibleCharacters = 0;

        while (ellipsisText.maxVisibleCharacters < ellipsisText.text.Length)
        {
            ellipsisText.maxVisibleCharacters++;
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(DisplayEllipsis());
    }
    
}
