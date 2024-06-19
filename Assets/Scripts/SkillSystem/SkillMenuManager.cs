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
    private bool[][] learnedSkills;
    private int[] skillTierUnlockLevels = {1, 2, 4, 7};

    private bool[] availableSkills = new bool[15];
    private bool[] selectedCharacterLearnedSkills;

    private int currentSkillIndex;

    private int playerLevel;
    private int selectedCharacter;

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
        Skill[] skills = playerScriptableObject.skills;

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
        titleText.text = playerScriptableObject.skills[currentSkillIndex].name;
        descriptionText.text = playerScriptableObject.skills[currentSkillIndex].skillDescription;

        // check if the skill can be learned
        if (availableSkills[currentSkillIndex] == true)
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
        selectedCharacterLearnedSkills[currentSkillIndex] = true;

        // what skills are available
        AvailableSkillList();

        // close the preview menu
        ClosePreview();

        // update skills availability
        transform.GetComponent<SkillUnlockManager>().ApplyPassiveSkills();
        transform.GetComponent<SkillUnlockManager>().ShowUnlockedButtons();
    }

    public void ClosePreview()
    {
        previewMenu.SetActive(false);
    }

    public void AvailableSkillList()
    {
        // find the last learned skill
        int lastLearnedSkill = -1;
        string lastLearnedSkillID = "";
        for (int i = 0; i < selectedCharacterLearnedSkills.Length; i++)
        {
            if (selectedCharacterLearnedSkills[i] == true)
            {
                lastLearnedSkill = i;
            }
        }

        if (lastLearnedSkill >= 0)
        {
            // get the last learned skill ID
            lastLearnedSkillID = skillIDs[lastLearnedSkill];

            // get player level
            playerLevel = transform.GetComponent<LevelManager>().GetPlayerLevel;

            // set all skills to unavailable
            for (int i = 0; i < skillButtons.Length; i++)
            {
                availableSkills[i] = false;
            }

            // loop through the skills
            for (int i = 0; i < skillButtons.Length; i++)
            {

                // set availability based on position in the skill tree and last learned skill
                if (skillIDs[i].StartsWith(lastLearnedSkillID) && skillIDs[i].Length == lastLearnedSkillID.Length + 2)
                {
                    availableSkills[i] = true;
                }
                else
                {
                    availableSkills[i] = false;
                }

                // check if the player can learn tier 1 skills
                if (skillIDs[i].Length == 3 && playerLevel < skillTierUnlockLevels[1])
                {
                    availableSkills[i] = false;
                }

                // check if the player can learn tier 2 skills
                if (skillIDs[i].Length == 5 && playerLevel < skillTierUnlockLevels[2])
                {
                    availableSkills[i] = false;
                }

                // check if the player can learn tier 3 skills
                if (skillIDs[i].Length == 7 && playerLevel < skillTierUnlockLevels[3])
                {
                    availableSkills[i] = false;
                }
            }
        }
        else
        {
            // get player level
            playerLevel = transform.GetComponent<LevelManager>().GetPlayerLevel;

            // set all skills to unavailable
            for (int i = 0; i < skillButtons.Length; i++)
            {
                availableSkills[i] = false;
            }

            // check if the player can learn tier 0 skills
            if (playerLevel >= skillTierUnlockLevels[0])
            {
                availableSkills[0] = true;
            }
        }

        ShowSkills();
    }

    private void ShowSkills()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (selectedCharacterLearnedSkills[i] == true)
            {
                //white
                skillButtons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else if (availableSkills[i] == true)
            {
                // blue
                skillButtons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 1, 1);
            }
            else
            {
                // grey
                skillButtons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
    }

    public void UnLearnAllSkills()
    {
        for (int i = 0; i < selectedCharacterLearnedSkills.Length; i++)
        {
            selectedCharacterLearnedSkills[i] = false;
            availableSkills[i] = false;
        }

        AvailableSkillList();

        // update skills availability
        transform.GetComponent<SkillUnlockManager>().ApplyPassiveSkills();
        transform.GetComponent<SkillUnlockManager>().ShowUnlockedButtons();

    }

    // getter for selectedcharacterlearnedskills
    public bool[] GetSelectedCharacterLearnedSkills
    {
        get { return selectedCharacterLearnedSkills; }
    }

    public void LoadData(GameData data)
    {
        this.learnedSkills = data.learnedSkills;
        this.selectedCharacter = data.selectedCharacter;


        this.selectedCharacterLearnedSkills = this.learnedSkills[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        data.learnedSkills = this.learnedSkills;
        data.selectedCharacter = this.selectedCharacter;


        this.learnedSkills[selectedCharacter] = this.selectedCharacterLearnedSkills;
    }
}
