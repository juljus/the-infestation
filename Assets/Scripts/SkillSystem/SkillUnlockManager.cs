using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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

        ApplyPassiveSkills();
        ShowUnlockedButtons();
    }

    public void ApplyPassiveSkills()
    {
        for (int i = 0; i < playerScriptableObject.skills.Length; i++)
        {
            if (playerScriptableObject.skills[i].isPassive && selectedCharacterLearnedSkills[i])
            {
                print("Activating skill " + i);
                playerScriptableObject.skills[i].Activate(transform.GetComponent<PlayerManager>().GetPlayer);
            }
            else if (playerScriptableObject.skills[i].isPassive && !selectedCharacterLearnedSkills[i])
            {
                print("Deactivating skill " + i);
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
                unlockedActiveSkills[buttonsUnlocked - 1] = i;

                switch (buttonsUnlocked)
                {
                    case 1:
                        skill0Button.GetComponent<UnityEngine.UI.Image>().sprite = playerScriptableObject.skills[i].skillIcon;

                        foreach (Transform child in skill0Button.transform)
                        {
                            Destroy(child.gameObject);
                        }

                        Instantiate(skill0Button.transform.GetComponent<UnityEngine.UI.Image>(), skill0Button.transform.position, skill0Button.transform.rotation, skill0Button.transform);
                        
                        skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        skill0Button.transform.GetChild(0).gameObject.SetActive(true);
                        skill0Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
                        break;
                    case 2:
                        skill1Button.GetComponent<UnityEngine.UI.Image>().sprite = playerScriptableObject.skills[i].skillIcon;

                        foreach (Transform child in skill1Button.transform)
                        {
                            Destroy(child.gameObject);
                        }

                        Instantiate(skill1Button.transform.GetComponent<UnityEngine.UI.Image>(), skill1Button.transform.position, skill1Button.transform.rotation, skill1Button.transform);
                        
                        skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        skill1Button.transform.GetChild(0).gameObject.SetActive(true);
                        skill1Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
                        break;
                    case 3:
                        skill2Button.GetComponent<UnityEngine.UI.Image>().sprite = playerScriptableObject.skills[i].skillIcon;
                        
                        foreach (Transform child in skill2Button.transform)
                        {
                            Destroy(child.gameObject);
                        }
                        
                        Instantiate(skill2Button.transform.GetComponent<UnityEngine.UI.Image>(), skill2Button.transform.position, skill2Button.transform.rotation, skill2Button.transform);
                        
                        skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        skill2Button.transform.GetChild(0).gameObject.SetActive(true);
                        skill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
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
    }

    public int[] GetUnlockedActiveSkills
    {
        get { return unlockedActiveSkills; }
    }
}
