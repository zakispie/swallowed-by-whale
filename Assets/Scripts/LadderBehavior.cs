using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBehavior : MonoBehaviour
{
    // Reference to player
    private GameObject player;

    private Rigidbody playerRB;

    private bool canEnterLadder;

    void Start() {
        player = GameObject.FindWithTag("Player");
        playerRB = player.GetComponent<Rigidbody>();
    }

     void Update()
    {
        // Is in ladder's area
        if (canEnterLadder)
        {
            // Player presses 'E' to interact
            if (Input.GetKeyDown(KeyCode.E))
            {
                // If on ladder, get off
                // If not on ladder, get on
                PlayerController._onLadder = !PlayerController._onLadder;
                
                if (PlayerController._onLadder)
                {
                    // Stops player's velocity to avoid shooting up when entering out the top
                    playerRB.velocity = Vector3.zero;
                }
            }
        }
    }

    // While player is in ladder area
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canEnterLadder = true;
        }
    }

    // When player leaves ladder area
    private void OnTriggerExit(Collider other)
    {
        canEnterLadder = false;
        PlayerController._onLadder = false;

        // Stops player's velocity to avoid shooting up when entering out the top
        playerRB.velocity = Vector3.zero;
    }
}
