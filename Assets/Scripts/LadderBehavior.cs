using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBehavior : MonoBehaviour
{
    // Reference to player
    public GameObject player;

    public Rigidbody playerRB;

    void Start() {
        playerRB = player.GetComponent<Rigidbody>();
    }

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

        PlayerController._onLadder = false;
        playerRB.velocity = Vector3.zero;

    }
}
