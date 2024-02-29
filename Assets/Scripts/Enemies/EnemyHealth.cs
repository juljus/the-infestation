using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject scriptableObject;
    [SerializeField] private UnityEngine.UI.Image healthBar;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        maxHealth = scriptableObject.maxHealth;
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Getters
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Setters
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
}
