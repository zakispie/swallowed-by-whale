using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Tooltip("E To Interact Text Reference")]
    [SerializeField] private TMP_Text UITextReference;

    // Start is called before the first frame update
    void Start()
    {
        UITextReference.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UITextReference.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UITextReference.enabled = false;
        }
    }
}
