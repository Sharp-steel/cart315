using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 2;
    public int currentHealth;

    public event Action OnDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
