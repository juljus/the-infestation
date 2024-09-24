using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    [SerializeField] private UnityEngine.UI.Image healthBar;

    private float health;
    [SerializeField] private float currentHealth;

    private float incomingDamageModForTier4Skills;

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

    private void AfterHealthChange()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthBar.fillAmount = 0;
            Death();
        }

        if (currentHealth >= health)
        {
            currentHealth = health;
            healthBar.fillAmount = 1;
        }

        // update health bar
        healthBar.fillAmount = currentHealth / health;
    }


    public void TakeDamage(float damage, bool byPass = false)
    {
        if (byPass)
        {
            currentHealth -= damage;
        }
        else
        {
            currentHealth -= damage * incomingDamageModForTier4Skills;
            lastDamageRecieved = damage;
            takeDamageEvent.Invoke();
        }

        AfterHealthChange();
    }

    


    public void Heal(float heal)
    {
        currentHealth += heal;

        AfterHealthChange();
    }


    // Getters
    public float GetCurrentHealth
    {
        get{ return currentHealth; }
    }

    public float GetMaxHealth
    {
        get{ return health; }
    }

    public float GetLastDamageRecieved
    {
        get{ return lastDamageRecieved; }
    }


    // Setters
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;

        AfterHealthChange();
    }

    public void SetIncomingDamageModForTier4Skills(float mod)
    {
        incomingDamageModForTier4Skills = mod;
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
