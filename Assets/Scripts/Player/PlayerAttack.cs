using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject attackBtn;

    [SerializeField] private float currentAttackDamage;
    [SerializeField] private float currentAttackTime;
    [SerializeField] private float currentAttackCooldown;

    private float attackDamage;
    private float attackTime;
    private float attackCooldown;
    private float attackRange;

    private GameObject attackButtonOverlay;
    GameObject gameManager;

    public UnityEvent playerAttackEvent = new UnityEvent();

    private bool isAttacking = false;
    private bool animationToCooldown = false;

    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        attackButtonOverlay = Instantiate(attackBtn, attackBtn.transform.position, attackBtn.transform.rotation, attackBtn.transform);
        attackButtonOverlay.GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        attackButtonOverlay.SetActive(false);
        attackButtonOverlay.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;

        currentAttackDamage = attackDamage;
        currentAttackTime = attackTime;
        currentAttackCooldown = attackCooldown;
    }


    // --------- PUBLIC FUNCTIONS --------- 
    public void Attack()
    {
        if (isAttacking) { return; }

        isAttacking = true;

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

        // start attack animation
        UnityEngine.UI.Image attackBtnOverlay = attackBtn.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(AttackTime(attackBtnOverlay));

        // attack
        playerAttackEvent.Invoke();
    }


    // --------- COROUTINES ---------
    private IEnumerator AttackTime(UnityEngine.UI.Image attackButtonOverlay)
    {
        float time = 0;

        while (time < currentAttackTime && !animationToCooldown)
        {
            time += Time.deltaTime;
            attackButtonOverlay.fillAmount = time / currentAttackTime;
            yield return null;
        }

        attackButtonOverlay.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
        StartCoroutine(AttackCooldown(attackButtonOverlay));
    }

    private IEnumerator AttackCooldown(UnityEngine.UI.Image attackButtonOverlay)
    {
        float time = 0;

        while (time < currentAttackCooldown && !animationToCooldown)
        {
            time += Time.deltaTime;
            attackButtonOverlay.fillAmount = 1 - (time / currentAttackCooldown);
            yield return null;
        }

        while (animationToCooldown && time < currentAttackCooldown + currentAttackTime)
        {
            time += Time.deltaTime;
            attackButtonOverlay.fillAmount = 1 - (time / (currentAttackCooldown + currentAttackTime));
            yield return null;
        }

        attackButtonOverlay.fillAmount = 0;
        isAttacking = false;
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

    public void SetAnimationToCooldown(bool newAnimationToCooldown)
    {
        animationToCooldown = newAnimationToCooldown;
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.attackDamage = data.playerAttackDamage[selectedCharacter];
        this.attackTime = data.playerAttackTime[selectedCharacter];
        this.attackRange = data.playerAttackRange[selectedCharacter];
        this.attackCooldown = data.playerAttackCooldown[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.playerAttackDamage[selectedCharacter] = this.attackDamage;
        data.playerAttackTime[selectedCharacter] = this.attackTime;
        data.playerAttackRange[selectedCharacter] = this.attackRange;
        data.playerAttackCooldown[selectedCharacter] = this.attackCooldown;
    }
}
