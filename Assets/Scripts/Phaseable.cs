using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{

    [Tooltip("How long the player has to go through the platform from underneat")]
    public float resolidifyTime = 0.5f;

    /* Top and bottom colliders */
    private Collider[] allColliders;
    private Collider topCollider;
    private Collider bottomCollider;

    /* Whether or not the player is underneath the platform and trying to
    jump through it */
    private bool isJumpingThrough = false;

    void Start() 
    {
        /* Get colliders 
            NOTE: Bottom collider is set to IsTrigger and top collider is not */
        allColliders = GetComponentsInChildren<Collider>();
        topCollider = allColliders[0];
        bottomCollider = allColliders[1];

    }

    void Update()
    {
        if(isJumpingThrough || Input.GetKeyDown(KeyCode.S))
        {
            /* Removes top collider so player can go through it */
            topCollider.enabled = false;
            /* Gives player time to travel through platform before becoming solid */
            Invoke("Resolidify", 0.75f);
            isJumpingThrough = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isJumpingThrough = true;
        }
    }

    void Resolidify()
    {
        topCollider.enabled = true;
    }

}