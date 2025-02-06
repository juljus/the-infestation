using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    [SerializeField] private UnityEngine.UI.Image healthBar;

    private float health;
    private float currentHealth;
    [SerializeField] private float currentShield;

    private float lastDamageRecieved;

    private bool invulnerable = false;

    public UnityEvent takeDamageEvent;

    void Start()
    {
        currentHealth = health;
    }


    private void Death()
    {
        print("Player died");
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


    public void TakeDamage(float damage)
    {
        print("take damage");
        if (invulnerable) { return; }

        if (currentShield > 0)
        {
            if (currentShield >= damage)
            {
                currentShield -= damage;
                // FIXME: update shield bar?
                return;
            }
            else
            {
                damage -= currentShield;
                currentShield = 0;
            }
        }

        currentHealth -= damage;
        lastDamageRecieved = damage;
        takeDamageEvent.Invoke();

        AfterHealthChange();
    }


    public void Heal(float heal)
    {
        print("heal");
        currentHealth += heal;

        AfterHealthChange();
    }


    //! getters
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


    //! setters
    public void SetShield(float shield)
    {
        print("noooooo");
        if (currentShield < shield)
        {
            print("yessss");
            currentShield = shield;
        }
    }

    public void SetCurrentHealth(float newHealth)
    {
        if (newHealth < currentHealth)
        {
            TakeDamage(currentHealth - newHealth);
        }
        else
        {
            Heal(newHealth - currentHealth);
        }
    }

    public void SetIfInvulnerable(bool val)
    {
        if (val)
        {
            invulnerable = true;
        }
        else
        {
            invulnerable = false;
        }
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.health = data.playerHealth[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedChar;

        data.playerHealth[selectedCharacter] = this.health;
    }
}
