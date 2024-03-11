using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SkillMenuManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject skillMenu;
    [SerializeField] private GameObject[] skillButtons;
    private PlayerScriptableObject playerScriptableObject;

    private int[] learnedSkills;

    private void Start()
    {
        playerScriptableObject = transform.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        SetSkillButtonIcons();

        learnedSkills = new int[14];
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

        // learn the skill
        
        // hide unavailable skill buttons
    }

    public void LoadData(GameData data)
    {
        this.learnedSkills = data.learnedSkills;
    }

    public void SaveData(ref GameData data)
    {
        data.learnedSkills = this.learnedSkills;
    }

}
