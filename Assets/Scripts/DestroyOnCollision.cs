using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Behavior that causes the attached object to be destroyed and damage dealt upon trigger enter
/// </summary>
public class DestroyOnCollision : MonoBehaviour
{
    [Tooltip("Can this bullet deal damage to the player?")]
    [SerializeField] private bool firedByEnemy;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!firedByEnemy && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(1);
        } else if (firedByEnemy && other.gameObject.CompareTag("Player"))
        {
            PlayerController.HealthBar.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
