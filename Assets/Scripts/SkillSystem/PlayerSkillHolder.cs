using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHolder : MonoBehaviour
{
    [SerializeField] private Skill skill0;
    [SerializeField] private GameObject skill0Button;
    
    [SerializeField] private Skill skill1;
    [SerializeField] private GameObject skill1Button;
    
    [SerializeField] private Skill skill2;
    [SerializeField] private GameObject skill2Button;

    private Coroutine skill0ActiveCoroutine;
    private Coroutine skill1ActiveCoroutine;
    private Coroutine skill2ActiveCoroutine;

    private Coroutine skill0CooldownCoroutine;
    private Coroutine skill1CooldownCoroutine;
    private Coroutine skill2CooldownCoroutine;

    private bool castInProgress = false;

    // 0 = ready, 1 = active, 2 = cooldown
    private List<int> skillStates = new List<int> {0, 0, 0};

    private int[] unlockedActiveSkills = new int[3];

    private SkillHelper skillHelper;

    void Start()
    {
        skillHelper = GameObject.Find("GameManager").GetComponent<SkillHelper>();
    }

    public void ResetAllSkills()
    {
        // stop all coroutines
        StopAllCoroutines();

        // set skill states to 0
        skillStates = new List<int> {0, 0, 0};

        // set castInProgress to false
        castInProgress = false;

        // set skill buttons to 0
        try
        {
            skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        }
        catch (UnityException)
        {
            print("Skill 0 button overlay probably not intantiated yet");
        }

        try
        {
            skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        }
        catch (UnityException)
        {
            print("Skill 1 button overlay probably not intantiated yet");
        }

        try
        {
            skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        }
        catch (UnityException)
        {
            print("Skill 2 button overlay probably not intantiated yet");
        }


        // skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        // skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        // skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;

        // if (skill0Button.transform.GetChild(0) != null)
        // {
        //     skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        // }
        // if (skill1Button.transform.GetChild(0) != null)
        // {
        //     skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        // }
        // if (skill2Button.transform.GetChild(0) != null)
        // {
        //     skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
        // }
    }

    public void SkipSkill0ActiveDuration()
    {
        if (skill0ActiveCoroutine != null)
        {
            StopCoroutine(skill0ActiveCoroutine);
            skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[0] = 2;
            StartCoroutine(CooldownDuration(skill0.cooldownTime, skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(), 0));
        }
    }

    public void SkipSkill1ActiveDuration()
    {
        if (skill1ActiveCoroutine != null)
        {
            StopCoroutine(skill1ActiveCoroutine);
            skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[1] = 2;
            StartCoroutine(CooldownDuration(skill1.cooldownTime, skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(), 1));
        }
    }

    public void SkipSkill2ActiveDuration()
    {
        if (skill2ActiveCoroutine != null)
        {
            StopCoroutine(skill2ActiveCoroutine);
            skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[2] = 2;
            StartCoroutine(CooldownDuration(skill2.cooldownTime, skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>(), 2));
        }
    }

    public Skill GetSkill0
    {
        get { return skill0; }
    }

    public Skill GetSkill1
    {
        get { return skill1; }
    }

    public Skill GetSkill2
    {
        get { return skill2; }
    }

    public void SkipSkill0Cooldown()
    {
        if (skillStates[0] == 2)
        {
            skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[0] = 0;

            // stop the cooldown coroutine
            StopCoroutine(skill0CooldownCoroutine);
        }
    }

    public void SkipSkill1Cooldown()
    {
        if (skillStates[1] == 2)
        {
            skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[1] = 0;

            // stop the cooldown coroutine
            StopCoroutine(skill1CooldownCoroutine);
        }
    }

    public void SkipSkill2Cooldown()
    {
        if (skillStates[2] == 2)
        {
            skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
            skillStates[2] = 0;

            // stop the cooldown coroutine
            StopCoroutine(skill2CooldownCoroutine);
        }
    }

    public void LearnSkills()
    {
        unlockedActiveSkills = GameObject.Find("GameManager").transform.GetComponent<SkillUnlockManager>().GetUnlockedActiveSkills;


        if (unlockedActiveSkills[0] != -1)
        {
            skill0 = GameObject.Find("GameManager").transform.GetComponent<PlayerManager>().GetPlayerScriptableObject.skills[unlockedActiveSkills[0]];
        }
        if (unlockedActiveSkills[1] != -1)
        {
            skill1 = GameObject.Find("GameManager").transform.GetComponent<PlayerManager>().GetPlayerScriptableObject.skills[unlockedActiveSkills[1]];
        }
        if (unlockedActiveSkills[2] != -1)
        {
            skill2 = GameObject.Find("GameManager").transform.GetComponent<PlayerManager>().GetPlayerScriptableObject.skills[unlockedActiveSkills[2]];
        }
    }


    public void ActivateSkill0()
    {
        int skillIndex = 0;

        // get isAttacking from PlayerAttack
        if (transform.GetComponent<PlayerAttack>().GetIsAttacking) { return; }

        // check if any other skill is being casted
        if (castInProgress) { return; }

        // check if skill is not off cooldown
        if (skillStates[skillIndex] != 0) { return; }


        TargetManager targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
        GameObject target = null;

        if (skill0.castRange != 0)
        {
            target = targetManager.GetTargetSmart();

            if (target == null)
            {
                // no enemies on map maybe?
                skillStates[skillIndex] = 0;
                return;
            }
        }

        if (skill0.castRange != 0 && Vector2.Distance(transform.position, target.transform.position) > skill0.castRange)
        {
            skillStates[skillIndex] = 0;
            return;
        }

        UnityEngine.UI.Image skill0ButtonOverlay = skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        skill0ButtonOverlay.fillAmount = 1;
        
        skill0.Activate(gameObject, skillHelper);
        skillStates[skillIndex] = 1;
        transform.GetComponent<PlayerAttack>().SetIsAttacking(true);
        castInProgress = true;
        skill0ActiveCoroutine = StartCoroutine(ActiveDuration(skill0.activeTime, skill0.cooldownTime, skill0.castRange, skill0ButtonOverlay, skillIndex));
    }

    public void ActivateSkill1()
    {
        int skillIndex = 1;

        // get isAttacking from PlayerAttack
        if (transform.GetComponent<PlayerAttack>().GetIsAttacking) { return; }

        // check if any other skill is being casted
        if (castInProgress) { return; }

        // check if skill is not off cooldown
        if (skillStates[skillIndex] != 0) { return; }


        TargetManager targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
        GameObject target = null;

        if (skill1.castRange != 0)
        {
            target = targetManager.GetTargetSmart();

            if (target == null)
            {
                // no enemies on map maybe?
                skillStates[skillIndex] = 0;
                return;
            }
        }

        if (skill1.castRange != 0 && Vector2.Distance(transform.position, target.transform.position) > skill1.castRange)
        {
            skillStates[skillIndex] = 0;
            return;
        }


        UnityEngine.UI.Image skill1ButtonOverlay = skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        skill1ButtonOverlay.fillAmount = 1;
        
        skill1.Activate(gameObject, skillHelper);
        skillStates[skillIndex] = 1;
        transform.GetComponent<PlayerAttack>().SetIsAttacking(true);
        castInProgress = true;
        skill1ActiveCoroutine = StartCoroutine(ActiveDuration(skill1.activeTime, skill1.cooldownTime, skill1.castRange, skill1ButtonOverlay, skillIndex));
    }

    public void ActivateSkill2()
    {
        int skillIndex = 2;

        // get isAttacking from PlayerAttack
        if (transform.GetComponent<PlayerAttack>().GetIsAttacking) { return; }

        // check if any other skill is being casted
        if (castInProgress) { return; }

        // check if skill is not off cooldown
        if (skillStates[skillIndex] != 0) { return; }

        TargetManager targetManager = GameObject.Find("GameManager").GetComponent<TargetManager>();
        GameObject target = null;

        if (skill2.castRange != 0)
        {
            target = targetManager.GetTargetSmart();

            if (target == null)
            {
                // no enemies on map maybe?
                skillStates[skillIndex] = 0;
                return;
            }
        }

        if (skill2.castRange != 0 && Vector2.Distance(transform.position, target.transform.position) > skill2.castRange)
        {
            skillStates[skillIndex] = 0;
            return;
        }


        UnityEngine.UI.Image skill2ButtonOverlay = skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        skill2ButtonOverlay.fillAmount = 1;
        
        skill2.Activate(gameObject, skillHelper);
        skillStates[skillIndex] = 1;
        transform.GetComponent<PlayerAttack>().SetIsAttacking(true);
        castInProgress = true;
        skill2ActiveCoroutine = StartCoroutine(ActiveDuration(skill2.activeTime, skill2.cooldownTime, skill2.castRange, skill2ButtonOverlay, skillIndex));
    }


    private IEnumerator ActiveDuration(float activeDuration, float cooldown, float castRange, UnityEngine.UI.Image buttonOverlay, int skillIndex)
    {
        yield return new WaitForSeconds(activeDuration);

        skillStates[skillIndex] = 2;
        transform.GetComponent<PlayerAttack>().SetIsAttacking(false);
        castInProgress = false;

        switch (skillIndex)
        {
            case 0:
                skill0CooldownCoroutine = StartCoroutine(CooldownDuration(cooldown, buttonOverlay, skillIndex));
                break;
            case 1:
                skill1CooldownCoroutine = StartCoroutine(CooldownDuration(cooldown, buttonOverlay, skillIndex));
                break;
            case 2:
                skill2CooldownCoroutine = StartCoroutine(CooldownDuration(cooldown, buttonOverlay, skillIndex));
                break;
        }
    }

    private IEnumerator CooldownDuration(float cooldown, UnityEngine.UI.Image buttonOverlay, int skillIndex)
    {
        float cooldownCurrent = cooldown;
        while (cooldownCurrent > 0)
        {
            cooldownCurrent -= Time.deltaTime;
            if (buttonOverlay != null)
            {
                buttonOverlay.fillAmount = cooldownCurrent / cooldown;
            }
            yield return null;
        }

        buttonOverlay.fillAmount = 0;
        skillStates[skillIndex] = 0;
    }

    public void UnlearnAllSkills()
    {
        skill0 = null;
        skill1 = null;
        skill2 = null;

        StopAllCoroutines();

        skillStates = new List<int> {0, 0, 0};
    }

    public GameObject GetSkill2Button
    {
        get { return skill2Button; }
    }

    public Skill GetSkillById(int id)
    {
        switch (id)
        {
            case 0:
                return skill0;
            case 1:
                return skill1;
            case 2:
                return skill2;
            default:
                return null;
        }
    }

    // GETTERS
    public bool GetCastInProgress
    {
        get { return castInProgress; }
    }
}
