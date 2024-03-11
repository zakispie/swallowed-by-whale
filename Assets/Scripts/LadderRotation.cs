using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderRotation : MonoBehaviour
{
    [Tooltip("Player Reference")]
    [SerializeField] private GameObject playerReference;

    [Tooltip("Distance For Animation Trigger")]
    [SerializeField] private float _distanceUntilAnimationTrigger = 1.0f;


    // Keyboard Property
    private static Keyboard Keyboard => PlayerController.Keyboard;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !PlayerController._onLadder)
        {
            Debug.Log("Getting on Ladder");
            playerReference.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && PlayerController._onLadder)
        {
            Debug.Log("Getting off Ladder");
            playerReference.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

}
