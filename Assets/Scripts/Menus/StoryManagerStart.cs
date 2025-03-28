using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManagerStart : MonoBehaviour
{
    [SerializeField] private string[] storyText;
    [SerializeField] private float[] storyTextDuration;

    [SerializeField] private TMPro.TMP_Text storyTextUI;

    [SerializeField] private GameObject skipButton;

    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private UnityEngine.UI.Image LoadingBar;

    private int currentStoryIndex = 0;

    private bool isSkipping = false;

    void Start()
    {
        // set alpha at 0
        storyTextUI.color = new Color(1, 1, 1, 0);

        StartCoroutine(ShowStoryText());
    }

    public void SkipStory()
    {
        StopAllCoroutines();

        // load game scene
        PersistentSceneManager.instance.LoadSceneWithLoadingScreen("StoryStart", "Game");
    }

    public void SkipOne()
    {
        isSkipping = true;
    }

    private IEnumerator ShowStoryText()
    {
        while (currentStoryIndex < storyText.Length)
        {
            // change text
            storyTextUI.text = storyText[currentStoryIndex];

            // wait for a bit
            yield return new WaitForSecondsRealtime(0.2f);

            // increase alpha gradually
            for (float i = 0; i < 1; i += Time.deltaTime * 2)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 1);

            // wait for duration
            float startTime = Time.realtimeSinceStartup;
            for (float i = Time.realtimeSinceStartup; i < startTime + storyTextDuration[currentStoryIndex]; i = Time.realtimeSinceStartup)
            {
                if (isSkipping)
                {
                    isSkipping = false;
                    break;
                }
                yield return null;
            }

            // decrease alpha gradually
            for (float i = 1; i > 0; i -= Time.deltaTime * 2)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 0);

            // increment index
            currentStoryIndex++;
        }

        // load game scene
        PersistentSceneManager.instance.LoadSceneWithLoadingScreen("StoryStart", "Game");
    }
}
