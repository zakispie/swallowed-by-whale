using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{

    public string[] allSceneDialouge;

    private bool hasHappenedThisRun;

    void Start()
    {
        hasHappenedThisRun = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasHappenedThisRun)
        {
            SoundManager.Instance.InitDialouge(allSceneDialouge, "Nontalking");
            hasHappenedThisRun = true;
        }
    }
}
