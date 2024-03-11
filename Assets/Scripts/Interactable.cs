using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Tooltip("Player Reference")]
    [SerializeField] private GameObject playerReference;

    [Tooltip("E To Interact Text Reference")]
    [SerializeField] private TMP_Text UITextReference;

    [Tooltip("Distance Until Iteractable")]
    [SerializeField] private float _distanceUntilInteractable = 4.0f;

    private float _distance;


    // Start is called before the first frame update
    void Start()
    {
        _distance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector3.Distance(this.transform.position, playerReference.transform.position);
        if(_distance <= _distanceUntilInteractable)
        {
            UITextReference.enabled = true;
        } else
        {
            UITextReference.enabled = false;
        }
    }
}
