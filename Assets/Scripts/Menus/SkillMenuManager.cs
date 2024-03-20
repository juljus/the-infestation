using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SkillMenuManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject skillMenu;
    [SerializeField] private GameObject[] skillButtons;
    [SerializeField] private string[] skillIDs;
    private PlayerScriptableObject playerScriptableObject;
    private int[] learnedSkills;
    private int[] shownSkills;

    private void Start()
    {
        playerScriptableObject = transform.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        SetSkillButtonIcons();

        ShowSkills();
    }

    private void SetSkillButtonIcons()
    {
        // get the skill icons
        Sprite[] skillIcons = playerScriptableObject.SkillIcons;

        // set the skill icons
        for (int i = 0; i < skillButtons.Length; i++)
        {
            try
            {
                skillButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = skillIcons[i];
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

    public void LearnSkill()
    {
        // get the button that activated the function
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        // get the index of the button
        int index = System.Array.IndexOf(skillButtons, button);

        // use the index to get the skill ID
        string skillID = skillIDs[index];

        // learn the skill
        learnedSkills[index] = 1;

        // make list of skills to show
        ShownSkillList(skillID);

        // show the skills
        ShowSkills();
    }

    private void ShownSkillList(string skillID)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            shownSkills[i] = 0;

            if (skillIDs[i].StartsWith(skillID) && skillIDs[i].Length <= skillID.Length + 2)
            {
                shownSkills[i] = 1;
            }
            else if (learnedSkills[i] == 1)
            {
                shownSkills[i] = 1;
            }
            else if (skillIDs[i].Length > skillID.Length + 2)
            {
                shownSkills[i] = 0;
            }
        }
    }

    private void ShowSkills()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (shownSkills[i] == 1 || i == 0)
            {
                skillButtons[i].SetActive(true);
            }
            else
            {
                skillButtons[i].SetActive(false);
            }
        }
    }

    public void UnLearnAllSkills()
    {
        for (int i = 0; i < learnedSkills.Length; i++)
        {
            learnedSkills[i] = 0;
            shownSkills[i] = 0;
        }

        ShowSkills();
    }

    public void LoadData(GameData data)
    {
        this.learnedSkills = data.learnedSkills;
        this.shownSkills = data.shownSkills;
    }

    public void SaveData(ref GameData data)
    {
        data.learnedSkills = this.learnedSkills;
        data.shownSkills = this.shownSkills;
    }

}
