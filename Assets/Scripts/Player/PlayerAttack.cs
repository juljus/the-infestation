using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IDataPersistance
{
    [SerializeField] private float currentAttackDamage;

    private float attackDamage;
    private float attackTime;
    
    
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
    }


    // GETTERS
    public float GetAttackDamage
    {
        get { return attackDamage; }
    }

    public float GetCurrentAttackDamage
    {
        get { return currentAttackDamage; }
    }


    // SETTERS
    public void SetCurrentAttackDamage(float newCurrentAttackDamage)
    {
        currentAttackDamage = newCurrentAttackDamage;
    }

    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.attackDamage = data.playerAttackDamage[selectedCharacter];
        this.attackTime = data.playerAttackTime[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.playerAttackDamage[selectedCharacter] = this.attackDamage;
        data.playerAttackTime[selectedCharacter] = this.attackTime;
    }
}
