using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject attackBtn;

    [SerializeField] private float currentAttackDamage;
    [SerializeField] private float currentAttackTime;

    private float attackDamage;
    private float attackTime;
    private float attackRange;

    private GameObject attackButtonOverlay;
    GameObject gameManager;

    public UnityEvent playerAttackEvent = new UnityEvent();

    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        attackButtonOverlay = Instantiate(attackBtn, attackBtn.transform.position, attackBtn.transform.rotation, attackBtn.transform);
        attackButtonOverlay.GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        attackButtonOverlay.SetActive(false);
        attackButtonOverlay.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
    }


    // --------- PUBLIC FUNCTIONS --------- 
    public void Attack()
    {
        GameObject target = gameManager.GetComponent<TargetManager>().GetTarget;

        if (target == null)
        {
            gameManager.GetComponent<TargetManager>().TargetClosestEnemy();
            target = gameManager.GetComponent<TargetManager>().GetTarget;
        }

        if (Vector2.Distance(transform.position, target.transform.position) > attackRange)
        {
            gameManager.GetComponent<TargetManager>().ClearTarget();
            return;
        }

        // attack
        playerAttackEvent.Invoke();
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

    public float GetAttackTime
    {
        get { return attackTime; }
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

    public void SetCurrentAttackTime(float newCurrentAttackTime)
    {
        currentAttackTime = newCurrentAttackTime;
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.attackDamage = data.playerAttackDamage[selectedCharacter];
        this.attackTime = data.playerAttackTime[selectedCharacter];
        this.attackRange = data.playerAttackRange[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.playerAttackDamage[selectedCharacter] = this.attackDamage;
        data.playerAttackTime[selectedCharacter] = this.attackTime;
        data.playerAttackRange[selectedCharacter] = this.attackRange;
    }
}
