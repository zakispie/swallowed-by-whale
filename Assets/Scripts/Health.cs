using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behavior that adds a health to a given object
/// </summary>
public class Health : MonoBehaviour
{
    [Tooltip("Starting Maximum Health")] [Min(0f)]
    [SerializeField] public float maxHealth;
    
    [Tooltip("Health Slider")]
    [SerializeField] private Slider healthSlider;

    // Tracks the current health of the object
    [NonSerialized] public float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;

    }
    
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        healthSlider.value = _currentHealth / maxHealth;
        
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
