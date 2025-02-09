using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    private int currentKills = 0;
    private LevelManager levelManager;
    private int level = 0;
    private int campfireStandingNextTo;

    [SerializeField] private GameObject[] campfires = new GameObject[3];
    [SerializeField] private GameObject campfireMenu;
    [SerializeField] private GameObject campfireMenuButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private int[] campfireKillThresholds = new int[4] { 0, 1, 2, 3 };

    private void Start()
    {
        levelManager = transform.GetComponent<LevelManager>();
        level = levelManager.GetPlayerLevel;
    }

    private void Update()
    {
        // check if standing near an unlocked campfire

    }

    public void AddKill()
    {
        currentKills++;
    }

    public void SetCampfireStandingNextTo(int campfireNum)
    {
        campfireStandingNextTo = campfireNum;
    }

    private void LightCampfires()
    {
        for (int i = 0; i < campfires.Length; i++)
        {
            if (i <= level)
            {
                // TODO: light the campfire
            }
            else
            {
                // TODO: extinguish the campfire
            }
        }
    }

    public void WTF()
    {
        print("WTF");
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
        // trigger InGameSave
        transform.GetComponent<DataPersistanceManager>().InGameSave();

        // reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

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
