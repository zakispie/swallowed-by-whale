using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.Die();
        }
    }

}
