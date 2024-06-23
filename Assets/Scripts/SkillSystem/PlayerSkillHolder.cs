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

    
    // 0 = ready, 1 = active, 2 = cooldown
    private List<int> skillStates = new List<int> {0, 0, 0};

    private int[] unlockedActiveSkills = new int[3];


    public void LearnSkills()
    {
        print("asdghpohvnp9aeij,vopia");
        unlockedActiveSkills = GameObject.Find("GameManager").transform.GetComponent<SkillUnlockManager>().GetUnlockedActiveSkills;

        print("unlockedActiveSkills: " + unlockedActiveSkills[0] + " " + unlockedActiveSkills[1] + " " + unlockedActiveSkills[2]);

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

        // ends function if skill is not ready
        if (skillStates[skillIndex] != 0) { return; }

        UnityEngine.UI.Image skill0ButtonOverlay = skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

        skill0ButtonOverlay.fillAmount = 1;
        
        skill0.Activate(gameObject);
        skillStates[skillIndex] = 1;
        StartCoroutine(ActiveDuration(skill0.activeTime, skill0.cooldownTime, skill0ButtonOverlay, skillIndex));
    }

    public void ActivateSkill1()
    {
        int skillIndex = 1;

        if (skillStates[skillIndex] != 0) { return; }

        UnityEngine.UI.Image skill1ButtonOverlay = skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

        skill1ButtonOverlay.fillAmount = 1;
        
        skill1.Activate(gameObject);
        skillStates[skillIndex] = 1;
        StartCoroutine(ActiveDuration(skill1.activeTime, skill1.cooldownTime, skill1ButtonOverlay, skillIndex));
    }

    public void ActivateSkill2()
    {
        int skillIndex = 2;

        if (skillStates[skillIndex] != 0) { return; }

        UnityEngine.UI.Image skill2ButtonOverlay = skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

        skill2ButtonOverlay.fillAmount = 1;
        
        skill2.Activate(gameObject);
        skillStates[skillIndex] = 1;
        StartCoroutine(ActiveDuration(skill2.activeTime, skill2.cooldownTime, skill2ButtonOverlay, skillIndex));
    }


    private IEnumerator ActiveDuration(float activeDuration, float cooldown, UnityEngine.UI.Image buttonOverlay, int skillIndex)
    {
        yield return new WaitForSeconds(activeDuration);

        skillStates[skillIndex] = 2;
        StartCoroutine(CooldownDuration(cooldown, buttonOverlay, skillIndex));
    }

    private IEnumerator CooldownDuration(float cooldown, UnityEngine.UI.Image buttonOverlay, int skillIndex)
    {
        float cooldownCurrent = cooldown;
        while (cooldownCurrent > 0)
        {
            cooldownCurrent -= Time.deltaTime;
            if (buttonOverlay != null)
            {
                if (skillIndex == 0)
                {
                }
                buttonOverlay.fillAmount = cooldownCurrent / cooldown;
            }
            yield return null;
        }

        skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
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
}
