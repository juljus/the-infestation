using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    private int currentKills = 0;
    private LevelManager levelManager;
    private int level = 0;
    private int campfireStandingNextTo;
    private GameObject player;

    [SerializeField] private GameObject[] campfires = new GameObject[3];
    [SerializeField] private GameObject campfireMenu;
    [SerializeField] private GameObject campfireMenuButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private int[] campfireKillThresholds = new int[4] { 0, 1, 2, 3 };
    [SerializeField] private UnityEngine.UI.Image blackoutImage;

    private void Start()
    {
        player = transform.GetComponent<PlayerManager>().GetPlayer;

        // start blackout animation
        StartCoroutine(Blackout());

        levelManager = transform.GetComponent<LevelManager>();
        level = levelManager.GetPlayerLevel;
        LightCampfires();
        UpdateCampfireKillCounter();
    }

    // TEMP: function for testing button
    public void SkipToGameEnd()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StoryEnd");
    }

    public void AddKill()
    {
        currentKills++;
        UpdateCampfireKillCounter();
    }

    public void SetCampfireStandingNextTo(int campfireNum)
    {
        campfireStandingNextTo = campfireNum;
    }

    private void LightCampfires()
    {
        level = levelManager.GetPlayerLevel;

        for (int i = 0; i < campfires.Length; i++)
        {
            if (i <= level)
            {
                // light campfire
                campfires[i].GetComponent<CampfireScript>().SetIfBurning(true);
                campfires[i].GetComponent<CampfireScript>().HideBar();
            }
            else
            {
                // extinguish campfire
                campfires[i].GetComponent<CampfireScript>().SetIfBurning(false);
                campfires[i].GetComponent<CampfireScript>().ShowBar();
            }
        }
    }

    private void UpdateCampfireKillCounter()
    {
        for (int i = 0; i < campfires.Length; i++)
        {
            float fillAmount;
            string text;

            if (currentKills < campfireKillThresholds[i])
            {
                fillAmount = (float)currentKills / campfireKillThresholds[i];
                text = currentKills + "/" + campfireKillThresholds[i];
            }
            else
            {
                fillAmount = 1;
                text = campfireKillThresholds[i] + "/" + campfireKillThresholds[i];
            }

            campfires[i].GetComponent<CampfireScript>().SetBarFillAndText(fillAmount, text);
        }

    }

    public void campfireMenuButtonPressed()
    {
        print("campfireMenuButtonPressed");
        
        level = levelManager.GetPlayerLevel;
        if (campfireStandingNextTo <= level)
        {
            print("You can interact with this campfire");
            CampfireMenuOn();
        }
        else if (campfireStandingNextTo == level + 1)
        {
            // check if enough kills
            if (currentKills >= campfireKillThresholds[campfireStandingNextTo])
            {
                print("You have enough kills to light this campfire");
                levelManager.GainLevel();
                LightCampfires();
            }
            else
            {
                print("You need more kills to light this campfire");
            }
        }
        else
        {
            print("You can't interact with this campfire yet");
        }
    }

    public void CampfireMenuOn()
    {
        campfireMenu.SetActive(true);

        // stop time
        Time.timeScale = 0;
    }

    public void CampfireMenuOff()
    {
        campfireMenu.SetActive(false);

        // start time
        Time.timeScale = 1;
    }

    public void RestAtCampfire()
    {
        // start blackout animation
        StartCoroutine(Blackout());

        // trigger InGameSave
        transform.GetComponent<DataPersistanceManager>().InGameSave();

        // replace all enemies
        transform.GetComponent<EnemyPlacementScript>().PlaceEnemies();

        // reset player health / cooldowns
        player.GetComponent<PlayerHealth>().ResetHealth();

        PlayerSkillHolder playerSkillHolder = player.transform.GetComponent<PlayerSkillHolder>();
        playerSkillHolder.ResetAllSkills();

        // campfire menu off
        CampfireMenuOff();
    }

    public void ShowCampfireMenuButton()
    {
        campfireMenuButton.SetActive(true);
        attackButton.SetActive(false);
    }

    public void HideCampfireMenuButton()
    {
        campfireMenuButton.SetActive(false);
        attackButton.SetActive(true);
    }

    private IEnumerator Blackout()
    {
        float alpha = 1;
        Time.timeScale = 0;

        blackoutImage.gameObject.SetActive(true);
        blackoutImage.color = new Color(0, 0, 0, alpha);

        float realTime = Time.realtimeSinceStartup;

        while(realTime + 0.5f > Time.realtimeSinceStartup)
        {
            yield return null;
        }

        Time.timeScale = 1;

        // start fade out
        while (alpha > 0)
        {
            alpha -= Time.unscaledDeltaTime * 4;
            blackoutImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackoutImage.gameObject.SetActive(false);
    }


    //! Data Persistance
    public void InGameSave(ref GameData data)
    {
        int selectedCharacter = data.selectedChar;

        data.charKills[selectedCharacter] = this.currentKills;
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.currentKills = data.charKills[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
