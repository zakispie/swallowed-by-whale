using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public class AgnesDialouge : MonoBehaviour
{

    public List<string> foundDialouge;
    public List<string> hasKeyDialouge;
    public List<string> hasNoKeyDialouge;
    public List<string> stillLookingDialouge;
    public List<string> foundKeyDialouge;
    public Sprite agnesPortrait;

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

                if(PlayerController._hasKey)
                {
                    dialouge = foundDialouge.Concat(hasKeyDialouge).ToList();
                }
                else
                {
                    dialouge = foundDialouge.Concat(hasNoKeyDialouge).ToList();
                }

            }
            else if (hasMet)
            {

                if(PlayerController._hasKey)
                {
                    dialouge = foundKeyDialouge;
                }
                else
                {
                    dialouge = stillLookingDialouge;
                }

            }

            SoundManager.Instance.InitDialouge(dialouge.ToArray(), agnesPortrait);
            hasMet = true;
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
