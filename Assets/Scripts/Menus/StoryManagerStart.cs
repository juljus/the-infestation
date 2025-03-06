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

    void Start()
    {
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
        // hide skip button
        skipButton.SetActive(false);

        // fade out text
        float currentAlpha = storyTextUI.color.a;
        for (float i = currentAlpha; i > 0; i -= Time.deltaTime)
        {
            storyTextUI.color = new Color(1, 1, 1, i);
            yield return null;
        }

        // wait for a bit
        yield return new WaitForSeconds(0.5f);

        // load game scene
        PersistentSceneManager.instance.LoadGameScene("StoryStart");
    }

    IEnumerator ShowStoryText()
    {
        // // start loading game scene
        // AsyncOperation gameSceneAsync = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        // gameSceneAsync.allowSceneActivation = false;

        while (currentStoryIndex < storyText.Length)
        {
            // change text
            storyTextUI.text = storyText[currentStoryIndex];

            // wait for a bit
            yield return new WaitForSecondsRealtime(1);

            // increase alpha gradually
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                storyTextUI.color = new Color(1, 1, 1, i);
                yield return null;
            }
            storyTextUI.color = new Color(1, 1, 1, 1);

            // wait for duration
            yield return new WaitForSecondsRealtime(storyTextDuration[currentStoryIndex]);

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

        // hide skip button
        skipButton.SetActive(false);

        // load game scene
        PersistentSceneManager.instance.LoadGameScene("StoryStart");

        // print("opening loading screen");

        // // set current progress as 0
        // float startProgress = gameSceneAsync.progress;
        // float currentProgress = gameSceneAsync.progress;
        // float endProgress = 0.9f;

        // // show loading screen with loading bar
        // LoadingScreen.SetActive(true);
        // LoadingBar.fillAmount = 0;
        

        // // wait for the game scene to be loaded
        // while (gameSceneAsync.progress < 0.9f || MapDiscoveryManager.gameStarting)
        // {
        //     print("load progress: " + gameSceneAsync.progress);

        //     // update bar based on keeping start and end progress in mind
        //     currentProgress = gameSceneAsync.progress;
        //     LoadingBar.fillAmount = Mathf.Lerp(startProgress, endProgress, currentProgress);
            
        //     yield return null;
        // }

        // print("at last! opening game scene");

        // gameSceneAsync.allowSceneActivation = true;

        // // unload this scene
        // SceneManager.UnloadSceneAsync("StoryStart");
    }
}
