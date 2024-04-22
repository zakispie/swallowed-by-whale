using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{

    public string[] allSceneDialouge;
    public Sprite playerPortait;
    private bool hasHappenedThisRun;

    void Start()
    {
        hasHappenedThisRun = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasHappenedThisRun)
        {
            SoundManager.Instance.InitDialouge(allSceneDialouge, playerPortait);
            hasHappenedThisRun = true;
        }
    }
}
