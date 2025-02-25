using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManagerEnd : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string[] storyText;
    [SerializeField] private float[] storyTextDuration;

    [SerializeField] private TMPro.TMP_Text storyTextUI;

    [SerializeField] private GameObject skipButton;

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

    public void SkipStory()
    {
        StopAllCoroutines();

        StartCoroutine(SkipStoryCoroutine());
    }

    IEnumerator SkipStoryCoroutine()
    {
        // fade out text
        float currentAlpha = storyTextUI.color.a;
        for (float i = currentAlpha; i > 0; i -= Time.deltaTime)
        {
            storyTextUI.color = new Color(1, 1, 1, i);
            yield return null;
        }

        // close skip button
        skipButton.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    IEnumerator ShowStoryText()
    {
        while (currentStoryIndex < storyText.Length)
        {
            // change text
            storyTextUI.text = storyText[currentStoryIndex];
            print("showing story text");

            // wait for a bit
            yield return new WaitForSeconds(1);
            print("waiting complete");

            // increase alpha gradually
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 1);

            // wait for duration
            yield return new WaitForSeconds(storyTextDuration[currentStoryIndex]);
            print("duration complete");

            // decrease alpha gradually
            for (float i = 1; i > 0; i -= Time.deltaTime)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 0);

            // increment index
            currentStoryIndex++;
            print("incremented index");
        }


        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
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
