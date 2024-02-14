using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling gun firing mechanics
/// </summary>
public class Gun : MonoBehaviour
{
    [Tooltip("Bullet Speed")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("Cooldown Between Shots (in seconds)")]
    [SerializeField] private float gunCooldown;
    
    [Tooltip("Bullet Prefab")]
    [SerializeField] private GameObject bulletObject;

    // Tracks whether the gun cooldown is ready to be fired
    private bool _readyToFire = true;

    // Tracks where the cooldown is in seconds (0 = complete)
    private float cooldownTime;

    // Adjusts size based on current health
    public Image cooldownBar;
    
    /// <summary>
    /// Handles player input
    /// </summary>
    void Update()
    {
        Debug.Log("Cool down time: " + cooldownTime);
        if (PlayerController.Mouse.leftButton.wasPressedThisFrame && _readyToFire)
        {
            _readyToFire = false;
            // instantiate a bulletObject and add force
            GameObject bullet = Instantiate(bulletObject, transform.position + PlayerController.FacingDirection.normalized, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(PlayerController.FacingDirection * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(EnableCooldownAfterTime());
        }

        if(cooldownTime > 0.0f){
            cooldownTime -= Time.deltaTime;
        } else {
            cooldownTime = 0.0f;
        }

        cooldownBar.fillAmount = cooldownTime / gunCooldown;
    }
    
    /// <summary>
    /// Waits for gun cooldown to finish, then enables firing
    /// </summary>
    /// <returns> Nothing </returns>
    private IEnumerator EnableCooldownAfterTime()
    {
        cooldownTime = gunCooldown;
        yield return new WaitForSeconds(gunCooldown);
        _readyToFire = true;
    }
}
