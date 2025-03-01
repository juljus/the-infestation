using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManagerStart : MonoBehaviour
{
    [SerializeField] private string[] storyText;
    [SerializeField] private float[] storyTextDuration;

    [SerializeField] private TMPro.TMP_Text storyTextUI;

    [SerializeField] private GameObject skipButton;

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
        // fade out text
        float currentAlpha = storyTextUI.color.a;
        for (float i = currentAlpha; i > 0; i -= Time.deltaTime)
        {
            storyTextUI.color = new Color(1, 1, 1, i);
            yield return null;
        }

        // close skip button
        skipButton.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
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

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
