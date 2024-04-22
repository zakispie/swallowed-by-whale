using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWall : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(PlayerController._hasKey)
        {
            Destroy(gameObject);
        }
    }
}
