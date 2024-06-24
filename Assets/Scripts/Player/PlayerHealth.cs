using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;
    [SerializeField] private UnityEngine.UI.Image healthBar;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        playerScriptableObject = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayerScriptableObject;

        maxHealth = playerScriptableObject.maxHealth;
        currentHealth = maxHealth;
    }


    private void Death()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // update health bar
        healthBar.fillAmount = currentHealth / maxHealth;
        
        // death
        if (currentHealth <= 0)
        {
            Death();
        }
    }


    public void Heal(float heal)
    {
        currentHealth += heal;

        // if health is above max health, set it to max health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // update health bar
        healthBar.fillAmount = currentHealth / maxHealth;
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
