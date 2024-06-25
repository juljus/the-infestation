using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;

    private float maxAttackDamage;
    [SerializeField] private float attackDamage;
    
    void Start()
    {
        // get player stuff
        GameObject gameManager = GameObject.Find("GameManager");
        playerScriptableObject = gameManager.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        maxAttackDamage = playerScriptableObject.attackDamage;
    }


    // GETTERS
    public float GetMaxAttackDamage
    {
        get { return maxAttackDamage; }
    }

    public float GetAttackDamage
    {
        get { return attackDamage; }
    }


    // SETTERS
    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
    }
}
