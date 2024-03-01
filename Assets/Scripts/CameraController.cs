using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset;
    float zPos;
    float ladderZPos;

    void Start()
    {
        // get and store the offset from the camera to the player
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        zPos = offset.z;
        ladderZPos = zPos + 1.5f;
    }

    void Update()
    {
        // add the offset to the player's position
        transform.position = player.transform.position + offset;
        
        if(PlayerController._onLadder)
        {
            offset.z = ladderZPos;
        } else {
            offset.z = zPos;
        }
        
    }
}
