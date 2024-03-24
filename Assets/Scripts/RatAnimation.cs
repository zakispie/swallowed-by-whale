using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimation : MonoBehaviour
{
    private Animator _animationController;

    // Start is called before the first frame update
    void Start()
    {
        _animationController = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
