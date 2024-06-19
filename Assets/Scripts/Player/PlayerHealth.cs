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

    void Update()
    {
        // update health bar
        healthBar.fillAmount = currentHealth / maxHealth;
        
        // death
        if (currentHealth <= 0)
        {
            Death();
        }

        // if health is above max health, set it to max health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // when x is pressed apply a slow effect to enemy
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("Enemy").GetComponent<EffectSystem>().TakeStatusEffect("dslIPDAGNJOIioiganhouinhIRHGU0ÜA.,os", "healthMod", -5, 10f);
        }

    }

    private void Death()
    {
        // destroy the player
        Destroy(gameObject);
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
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
