using System.Collections;
using System.Collections.Generic;
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


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Death();
        }

    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.fillAmount = currentHealth / maxHealth;        
    }

    private void Death()
    {
        Destroy(gameObject);

        GameObject.Find("GameManager").GetComponent<MapCompletion>().AddKill();
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
