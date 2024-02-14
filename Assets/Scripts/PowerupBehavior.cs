using UnityEngine;

// There's room for improvement in this script.
public class PowerupBehavior : MonoBehaviour
{
    private bool _touchingPlayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _touchingPlayer = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _touchingPlayer = false;
        }
    }

    void Update()
    {
        if (_touchingPlayer && PlayerController.Keyboard.eKey.wasPressedThisFrame)
        {
            CollectPowerup();
        }
    }

    void CollectPowerup()
    {
        // PlayerController.ActivateSpeedPowerup();
        Destroy(gameObject);
    }
}
