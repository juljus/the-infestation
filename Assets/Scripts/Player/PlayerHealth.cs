using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDataPersistance
{
    [SerializeField] private UnityEngine.UI.Image healthBar;
    [SerializeField] private UnityEngine.UI.Image shieldBar;

    private float health;
    private float currentHealth;
    [SerializeField] private float currentShield;

    private float lastDamageRecieved;

    private bool invulnerable = false;
    private GameObject gameManager;

    public UnityEvent takeDamageEvent;

    void Start()
    {
        currentHealth = health;
        gameManager = GameObject.Find("GameManager");
    }


    private void Death()
    {
        gameManager.GetComponent<GameOverManager>().GameOver();
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

        // update health and shield bar
        healthBar.fillAmount = currentHealth / health;
        if (currentShield <= health)
        {
            shieldBar.fillAmount = currentShield / health;
        }
        else
        {
            shieldBar.fillAmount = 1;
        }
    }


    public void TakeDamage(float damage)
    {
        if (invulnerable) { return; }

        if (currentShield > 0)
        {
            if (currentShield >= damage)
            {
                currentShield -= damage;
                AfterHealthChange();
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
        if (currentShield < shield)
        {
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

    public void ResetHealth()
    {
        currentHealth = health;
        currentShield = 0;
        AfterHealthChange();
    }

    //! IDataPersistance
    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.health = data.playerHealth[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
