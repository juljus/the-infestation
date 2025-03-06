using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockManager : MonoBehaviour
{
    [SerializeField] private GameObject skill0Button;
    [SerializeField] private GameObject skill1Button;
    [SerializeField] private GameObject skill2Button;

    private bool[] selectedCharacterLearnedSkills;
    private PlayerScriptableObject playerScriptableObject;

    private int[] unlockedActiveSkills = {-1, -1, -1};

    void Start() 
    {
        selectedCharacterLearnedSkills = transform.GetComponent<SkillMenuManager>().GetSelectedCharacterLearnedSkills;
        playerScriptableObject = transform.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        ShowUnlockedButtons();
        ApplyPassiveSkills();
    }

    public void ApplyPassiveSkills()
    {
        for (int i = 0; i < playerScriptableObject.skills.Length; i++)
        {
            if (playerScriptableObject.skills[i].isPassive && selectedCharacterLearnedSkills[i])
            {
                playerScriptableObject.skills[i].Activate(transform.GetComponent<PlayerManager>().GetPlayer, GetComponent<SkillHelper>());
            }
            else if (playerScriptableObject.skills[i].isPassive && !selectedCharacterLearnedSkills[i])
            {
                playerScriptableObject.skills[i].Deactivate(transform.GetComponent<PlayerManager>().GetPlayer);
            }
        }
    }

    public void ShowUnlockedButtons()
    {
        int buttonsUnlocked = 0;
        for (int i = 0; i < unlockedActiveSkills.Length; i++)
        {
            unlockedActiveSkills[i] = -1;
        }

        for (int i = 0; i < playerScriptableObject.skills.Length; i++)
        {
            if (selectedCharacterLearnedSkills[i] && playerScriptableObject.skills[i].isPassive == false)
            {
                buttonsUnlocked++;
                try
                {
                    unlockedActiveSkills[buttonsUnlocked - 1] = i;
                }
                catch (System.IndexOutOfRangeException)
                {
                    Debug.LogError("No more then 3 active skills can be unlocked! Also for some reason in this case the game will not load any skill buttons when it starts up.");
                }

                switch (buttonsUnlocked)
                {
                    case 1:
                        if (transform.GetComponent<PlayerManager>().GetPlayer.GetComponent<PlayerSkillHolder>().GetSkill0 == null)
                        {
                            StartCoroutine(SkillButtonOperations(skill0Button, i));
                        }
                        break;
                    case 2:
                        if (transform.GetComponent<PlayerManager>().GetPlayer.GetComponent<PlayerSkillHolder>().GetSkill1 == null)
                        {
                            StartCoroutine(SkillButtonOperations(skill1Button, i));
                        }
                        break;
                    case 3:
                        if (transform.GetComponent<PlayerManager>().GetPlayer.GetComponent<PlayerSkillHolder>().GetSkill2 == null)
                        {
                            StartCoroutine(SkillButtonOperations(skill2Button, i));
                        }
                        break;
                }
            }
        }

        switch (buttonsUnlocked)
        {
            case 0:
                skill0Button.SetActive(false);
                skill1Button.SetActive(false);
                skill2Button.SetActive(false);
                break;
            case 1:
                skill0Button.SetActive(true);
                skill1Button.SetActive(false);
                skill2Button.SetActive(false);
                break;
            case 2:
                skill0Button.SetActive(true);
                skill1Button.SetActive(true);
                skill2Button.SetActive(false);
                break;
            case 3:
                skill0Button.SetActive(true);
                skill1Button.SetActive(true);
                skill2Button.SetActive(true);
                break;
        }

        transform.GetComponent<PlayerManager>().GetPlayer.GetComponent<PlayerSkillHolder>().LearnSkills();
    }

    private IEnumerator SkillButtonOperations(GameObject button, int i) 
    {
        if (button.transform.childCount > 0)
        {
            Destroy(button.transform.GetChild(0).gameObject);
        }

        yield return new WaitForEndOfFrame();

        button.GetComponent<UnityEngine.UI.Image>().sprite = playerScriptableObject.skills[i].skillIcon;

        Instantiate(button, button.transform.position, button.transform.rotation, button.transform);
        
        button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
        button.transform.GetChild(0).gameObject.SetActive(true);
        button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    public int[] GetUnlockedActiveSkills
    {
        get { return unlockedActiveSkills; }
    }
}
