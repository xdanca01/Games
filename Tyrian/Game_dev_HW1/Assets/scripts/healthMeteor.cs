using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthMeteor : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private HealthBar healthBar;
    private float _currentHealth;
    private float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            healthBar.SetFillLevel(_currentHealth / maxHealth);
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void DealDamage(float damage)
    {
        CurrentHealth -= damage;
    }

}
