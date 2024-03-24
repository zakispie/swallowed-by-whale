using UnityEngine;
using UnityEngine.Serialization;

// There's room for improvement in this script.
public class PowerupBehavior : MonoBehaviour
{
    // TODO abstract to be used in other powerups in the future
    private bool _touchingPlayer;
    [SerializeField] private PlayerController.PowerupType powerupType;
    // [SerializeField] private GameObject pickUpEffect;
    // Functionality for particle effect on pickup is here whenever we want to add it, just uncomment

    /// <summary>
    /// Marks the powerup as touching the player
    /// </summary>
    /// <param name="other"> The collider that entered the trigger </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _touchingPlayer = true;
            
           // Instantiate(pickUpEffect, transform.position, transform.rotation);
        }
    }
    
    /// <summary>
    /// Marks the powerup as no longer touching the player
    /// </summary>
    /// <param name="other"> The collider that exited the trigger </param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _touchingPlayer = false;
        }
    }

    /// <summary>
    /// Handles keyboard input for collecting the powerup
    /// </summary>
    void Update()
    {
        if (_touchingPlayer && PlayerController.Keyboard.eKey.wasPressedThisFrame)
        {
            SoundManager.Instance.PlayPowerupSFX();
            CollectPowerup();
        }
    }

    /// <summary>
    /// Indicates that the player has collected the powerup to the player controller
    /// </summary>
    void CollectPowerup()
    {
        PlayerController.ApplyPowerup(powerupType);
        Destroy(gameObject);
    }
}
