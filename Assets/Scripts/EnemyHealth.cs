using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyStatsScriptableObject enemyStats;
    [SerializeField] private UnityEngine.UI.Image healthBar;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        maxHealth = enemyStats.maxHealth;
        currentHealth = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            print("Enemy died");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            print("Enemy took 10 damage");
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
}
