using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }
}
