using UnityEngine;

/// <summary>
/// Behavior that causes the attached object to be destroyed upon trigger enter
/// </summary>
public class DestroyOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
