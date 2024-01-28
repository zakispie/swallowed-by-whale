using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset;

    void Start()
    {
        // get and store the offset from the camera to the player
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    
    }

    void Update()
    {
        // add the offset to the player's position
        transform.position = player.transform.position + offset;
        
        // assign it to the camera's position
        
    }
}
