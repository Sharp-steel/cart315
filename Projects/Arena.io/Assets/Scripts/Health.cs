using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 2;
    public int currentHealth;

    public event Action OnDeath;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.darkRed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
        if (spriteRenderer != null)
        {
            StartCoroutine(Flash());
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator Flash()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
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
