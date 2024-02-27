using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : MonoBehaviour, IDataPersistance
{
    [SerializeField] private PlayerScriptableObject scriptableObject;
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

    public void LoadData(GameData data) {
        this.currentHealth = data.playerHealth;
    }

    public void SaveData(ref GameData data) {
        data.playerHealth = this.currentHealth;
    }

}
