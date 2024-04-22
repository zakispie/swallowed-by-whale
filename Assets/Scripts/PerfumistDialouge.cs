using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfumistDialouge : MonoBehaviour
{
    public List<string> foundDialouge;
    public List<string> hasMetDialouge;
    public Sprite perfumistPortrait;

    private bool hasMet;
    private bool inConvo;

    void Start()
    {
        hasMet = false;
        inConvo = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !inConvo)
        {
            List<string> dialouge = new List<string>();
            inConvo = true;

            if(!hasMet)
            {

                dialouge = foundDialouge;
                PlayerController._hasKey = true;

            }
            else if (hasMet)
            {

                dialouge = hasMetDialouge;

            }

            SoundManager.Instance.InitDialouge(dialouge.ToArray(), perfumistPortrait);
            hasMet = true;
            PlayerController._hasKey = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inConvo)
        {
            inConvo = false;
        }
    }
}
