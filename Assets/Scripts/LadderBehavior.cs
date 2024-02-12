using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBehavior : MonoBehaviour
{
    // Reference to player
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {

        // When player is on a ladder, set player variable _onLadder to true
        if (other.CompareTag("Player"))
        {
            PlayerController._onLadder = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        // When player gets off ladder, set player variable _onLadder to false
        if (other.CompareTag("Player"))
        {
            PlayerController._onLadder = false;
        }

    }
}
