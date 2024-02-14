using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling Player health bar
/// </summary>
public class HealthBar : MonoBehaviour
{
    [Tooltip("Starting Maximum Health")] [Min(0f)]
    [SerializeField] public float maxHealth;
    
    // Adjusts size based on current health
    public Image healthBar;

    // Tracks the current health of the object
    [NonSerialized] public float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        healthBar.fillAmount = _currentHealth / maxHealth;
        
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
