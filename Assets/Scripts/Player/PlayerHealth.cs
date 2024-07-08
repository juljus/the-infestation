using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    [SerializeField] private UnityEngine.UI.Image healthBar;

    private float health;
    private float currentHealth;

    private float incomingDamageModForDispersion;

    private float lastDamageRecieved;

    public UnityEvent takeDamageEvent;

    void Start()
    {
        currentHealth = health;
    }


    private void Death()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage * incomingDamageModForDispersion;

        lastDamageRecieved = damage;

        takeDamageEvent.Invoke();

        // update health bar
        healthBar.fillAmount = currentHealth / health;
        
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
        if (currentHealth > health)
        {
            currentHealth = health;
        }

        // update health bar
        healthBar.fillAmount = currentHealth / health;
    }


    // Getters
    public float GetCurrentHealth
    {
        get{ return currentHealth; }
    }

    public float GetLastDamageRecieved
    {
        get{ return lastDamageRecieved; }
    }


    // Setters
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void SetIncomingDamageModForDispersion(float mod)
    {
        incomingDamageModForDispersion = mod;
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.health = data.playerHealth[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.playerHealth[selectedCharacter] = this.health;
    }
}
