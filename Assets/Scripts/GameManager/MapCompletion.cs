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
    [SerializeField] private UnityEngine.UI.Image whiteoutImage;
    private GameObject bossPrefab;
    private Vector3 bossCoords;
    private GameObject boss;

    private void Start()
    {
        // set boss
        bossCoords = GameObject.FindGameObjectWithTag("Boss").transform.position;
        bossPrefab = GameObject.FindGameObjectWithTag("Boss");
        
        // spawn boss
        boss = Instantiate(bossPrefab, bossCoords, Quaternion.identity);
        GameObject structures = GameObject.Find("Structures");
        boss.transform.SetParent(structures.transform);

        // set boss prefab inactive
        if (bossPrefab != null)
        {
            bossPrefab.SetActive(false);
        }


        player = transform.GetComponent<PlayerManager>().GetPlayer;

        levelManager = transform.GetComponent<LevelManager>();
        level = levelManager.GetPlayerLevel;
        LightCampfires();
        UpdateCampfireKillCounter();
    }

    public void EndSequence()
    {
        // whiteout image
        whiteoutImage.color = new Color(1, 1, 1, 0);
        whiteoutImage.gameObject.SetActive(true);

        // time scale 0
        Time.timeScale = 0;

        // save
        transform.GetComponent<DataPersistanceManager>().InGameSave();

        // start end sequence coroutine
        StartCoroutine(EndSequenceCoroutine());
    }

    private IEnumerator EndSequenceCoroutine()
    {
        // time stuff
        float startTime = Time.realtimeSinceStartup;

        // over 5 seconds whiteout image fades in
        float duration = 5f;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            // using real time so that the fade in is not affected by time scale
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = elapsed / duration;
            whiteoutImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // wait 2 seconds
        yield return new WaitForSecondsRealtime(2f);

        // then fade to black over 1 second
        duration = 1f;
        startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            // using real time so that the fade in is not affected by time scale
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = elapsed / duration;
            whiteoutImage.color = new Color(1 - alpha, 1 - alpha, 1 - alpha, 1);
            yield return null;
        }

        // load end scene
        PersistentSceneManager.instance.LoadScene("Game", "StoryEnd");
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
        level = levelManager.GetPlayerLevel;
        if (campfireStandingNextTo <= level)
        {
            CampfireMenuOn();
        }
        else if (campfireStandingNextTo == level + 1)
        {
            // check if enough kills
            if (currentKills >= campfireKillThresholds[campfireStandingNextTo])
            {
                levelManager.GainLevel();
                LightCampfires();
            }
            else
            {
                // not enough kills
            }
        }
        else
        {
            // not next to a campfire
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

    public void ResetBoss()
    {
        // delete boss minions and boss
        GameObject[] bossMinions = GameObject.FindGameObjectsWithTag("Minion");
        if (bossMinions.Length > 0)
        {
            foreach (GameObject minion in bossMinions)
            {
                Destroy(minion);
            }
        }

        if (boss != null)
        {
            Destroy(boss);
        }

        // spawn boss
        boss = Instantiate(bossPrefab, bossCoords, Quaternion.identity);
        GameObject structures = GameObject.Find("Structures");
        boss.transform.SetParent(structures.transform);
        boss.SetActive(true);
    }

    public void RestAtCampfire()
    {
        // start coroutine
        StartCoroutine(RestAtCampfireCoroutine());
    }

    private IEnumerator RestAtCampfireCoroutine()
    {
        // start blackout
        float alpha = 1;
        Time.timeScale = 0;

        blackoutImage.gameObject.SetActive(true);
        blackoutImage.color = new Color(0, 0, 0, alpha);

        float realTime = Time.realtimeSinceStartup;

        while (realTime + 0.5f > Time.realtimeSinceStartup)
        {
            // set alpha
            alpha = 1 - (realTime + 0.5f - Time.realtimeSinceStartup) * 2;
            blackoutImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        blackoutImage.color = new Color(0, 0, 0, 1);

        // wait for a bit
        yield return new WaitForSecondsRealtime(0.2f);

        // trigger InGameSave
        transform.GetComponent<DataPersistanceManager>().InGameSave();

        // replace all enemies
        transform.GetComponent<EnemyPlacementScript>().PlaceEnemies();

        // reset boss
        ResetBoss();

        // reset player health / cooldowns
        player.GetComponent<PlayerHealth>().ResetHealth();

        PlayerSkillHolder playerSkillHolder = player.transform.GetComponent<PlayerSkillHolder>();
        playerSkillHolder.ResetAllSkills();

        PlayerAttack playerAttack = player.transform.GetComponent<PlayerAttack>();
        playerAttack.SetIsAttacking(false);

        // campfire menu off
        CampfireMenuOff();

        // start fade out
        realTime = Time.realtimeSinceStartup;

        while (realTime + 0.5f > Time.realtimeSinceStartup)
        {
            // set alpha
            alpha = (realTime + 0.5f - Time.realtimeSinceStartup) * 2;
            blackoutImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        Time.timeScale = 1;
        blackoutImage.gameObject.SetActive(false);

        yield return null;
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
