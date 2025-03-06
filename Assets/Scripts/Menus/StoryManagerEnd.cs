using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManagerEnd : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string[] storyText;
    [SerializeField] private float[] storyTextDuration;

    [SerializeField] private TMPro.TMP_Text storyTextUI;

    private int currentStoryIndex = 0;

    void Start()
    {
        Time.timeScale = 1;

        // trigger save game
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // set alpha at 0
        storyTextUI.color = new Color(1, 1, 1, 0);

        StartCoroutine(ShowStoryText());
    }

    IEnumerator ShowStoryText()
    {
        while (currentStoryIndex < storyText.Length)
        {
            // change text
            storyTextUI.text = storyText[currentStoryIndex];

            // wait for a bit
            yield return new WaitForSeconds(1);

            // increase alpha gradually
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 1);

            // wait for duration
            yield return new WaitForSeconds(storyTextDuration[currentStoryIndex]);

            // decrease alpha gradually
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 0);

            // increment index
            currentStoryIndex++;
        }
        
        // wait for a bit
        yield return new WaitForSeconds(1);

        // load pregame scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
        PersistentSceneManager.instance.LoadScene("StoryEnd", "PreGame");
    }

    //! IDataPersistance
    public void LoadData(GameData data)
    {
    }

    public void SaveData(ref GameData data)
    {
        int selectedChar = data.selectedChar;

        data.charDead[selectedChar] = true;
    }

    public void InGameSave(ref GameData data)
    {
    }
}
