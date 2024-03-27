using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillMenuManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject skillMenu;
    [SerializeField] private GameObject[] skillButtons;
    [SerializeField] private string[] skillIDs;
    private PlayerScriptableObject playerScriptableObject;
    private int[] learnedSkills;
    private int[] availableSkills;

    private int currentSkillIndex;

    [Header("SkillPreview")]
    [SerializeField] private GameObject previewMenu;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject learnButton;

    private void Start()
    {
        playerScriptableObject = transform.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        SetSkillButtonIcons();

        AvailableSkillList();
    }

    private void SetSkillButtonIcons()
    {
        // get the skills
        SkillScriptableObject[] skills = playerScriptableObject.skills;

        // set the skill icons
        for (int i = 0; i < skillButtons.Length; i++)
        {
            try
            {
                skillButtons[i].GetComponent<Image>().sprite = skills[i].skillIcon;
            }
            catch(System.IndexOutOfRangeException)
            {
                Debug.LogError("Ability icon not found for skill " + i);
            }
        }
    }
    public void BackToPauseMenu()
    {
        skillMenu.SetActive(false);
    }

    public void OpenPreview()
    {
        // get the button that activated the function
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        // get the index of the button
        currentSkillIndex = System.Array.IndexOf(skillButtons, button);

        // open the preview menu
        previewMenu.SetActive(true);

        // make list of available skills
        AvailableSkillList();

        // set preview menu text
        titleText.text = playerScriptableObject.skills[currentSkillIndex].skillName;
        descriptionText.text = playerScriptableObject.skills[currentSkillIndex].skillDescription;

        // check if the skill can be learned
        if (availableSkills[currentSkillIndex] == 1)
        {
            learnButton.SetActive(true);
        }
        else
        {
            learnButton.SetActive(false);
        }
    }

    public void LearnSkill()
    {
        // use the index to get the skill ID
        string skillID = skillIDs[currentSkillIndex];

        // learn the skill
        learnedSkills[currentSkillIndex] = 1;

        // what skills are available
        AvailableSkillList();

        // close the preview menu
        ClosePreview();
    }

    public void ClosePreview()
    {
        previewMenu.SetActive(false);
    }

    private void AvailableSkillList()
    {
        // find the last learned skill
        int lastLearnedSkill = -1;
        string lastLearnedSkillID = "";
        for (int i = 0; i < learnedSkills.Length; i++)
        {
            if (learnedSkills[i] == 1)
            {
                lastLearnedSkill = i;
            }
        }

        if (lastLearnedSkill >= 0)
        {
            lastLearnedSkillID = skillIDs[lastLearnedSkill];
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (skillIDs[i].StartsWith(lastLearnedSkillID) && skillIDs[i].Length == lastLearnedSkillID.Length + 2)
                {
                    availableSkills[i] = 1;
                }
                else
                {
                    availableSkills[i] = 0;
                }
            }
        }
        else
        {
            availableSkills[0] = 1;
        }

        ShowSkills();
    }

    private void ShowSkills()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (availableSkills[i] == 0)
            {
                skillButtons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
            else
            {
                skillButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void UnLearnAllSkills()
    {
        for (int i = 0; i < learnedSkills.Length; i++)
        {
            learnedSkills[i] = 0;
            availableSkills[i] = 0;
        }

        AvailableSkillList();
    }

    public void LoadData(GameData data)
    {
        this.learnedSkills = data.learnedSkills;
        this.availableSkills = data.shownSkills;
    }

    public void SaveData(ref GameData data)
    {
        data.learnedSkills = this.learnedSkills;
        data.shownSkills = this.availableSkills;
    }

}
