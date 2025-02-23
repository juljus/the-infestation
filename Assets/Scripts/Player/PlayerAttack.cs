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
        if (transform.GetComponent<PlayerLogic>().GetIsStunned > 0) { return; }
        if (isAttacking) { return; }

        print("attack initiated");

        isAttacking = true;

        GameObject target = gameManager.GetComponent<TargetManager>().GetTargetSmart();

        if (target == null)
        {
            // no enemies on map maybe?
            isAttacking = false;
            return;
        }

        if (Vector2.Distance(transform.position, target.transform.position) > attackRange)
        {
            isAttacking = false;
            return;
        }

        // start attack animation
        UnityEngine.UI.Image attackBtnOverlay = attackBtn.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(AttackTime(attackBtnOverlay, target));
    }


    // --------- COROUTINES ---------
    private IEnumerator AttackTime(UnityEngine.UI.Image attackButtonOverlay, GameObject target)
    {
        float time = 0;

        while (time < currentAttackTime && !animationToCooldown)
        {
            if (target == null)
            {
                attackButtonOverlay.fillAmount = 0;
                isAttacking = false;
                yield break;
            }

            if (Vector2.Distance(transform.position, target.transform.position) > attackRange)
            {
                attackButtonOverlay.fillAmount = 0;
                isAttacking = false;
                yield break;
            }

            time += Time.deltaTime;
            attackButtonOverlay.fillAmount = time / currentAttackTime;
            yield return null;
        }

        // apply damage
        if (target == null)
        {
            attackButtonOverlay.fillAmount = 0;
            isAttacking = false;
            yield break;
        }

        if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.GetComponent<EnemyBrain>().TakeDamage(currentAttackDamage);

            // invoke event
            playerAttackEvent.Invoke();
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

    public void SetCurrentAttackTime(float newCurrentAttackTime)
    {
        currentAttackTime = newCurrentAttackTime;
    }

    public void SetAnimationToCooldown(bool newAnimationToCooldown)
    {
        animationToCooldown = newAnimationToCooldown;
    }

    //! IDataPersistance
    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.attackDamage = data.playerAttackDamage[selectedCharacter];
        this.attackTime = data.playerAttackTime[selectedCharacter];
        this.attackRange = data.playerAttackRange[selectedCharacter];
        this.attackCooldown = data.playerAttackCooldown[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
