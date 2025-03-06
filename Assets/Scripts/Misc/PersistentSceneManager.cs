using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private Camera loadingCamera;

    [SerializeField] private UnityEngine.UI.Image blackScreen;

    private float fadeDuration = 0.2f;

    // LOADING ANIMATION
    [Header("Loading Animation")]
    [SerializeField] private GameObject starParent;
    [SerializeField] private GameObject starGO;

    [SerializeField] private int starCount;
    [SerializeField] private float starMinCycleTime;
    [SerializeField] private int starsReplacedPerFrame;

    [SerializeField] private float starMoveRadius;
    [SerializeField] private float starPlaceRadius;

    [SerializeField] private float moveAmountPerFrame;

    private GameObject[] stars;
    private Vector2 currentPos;
    // LOADING ANIMATION

    private void Start()
    {
        // LOADING ANIMATION

        // start at a random position on the circle of starMoveRadius
        float angle = Random.Range(0, 360);
        currentPos = new Vector2(Mathf.Cos(angle) * starMoveRadius, Mathf.Sin(angle) * starMoveRadius);

        // intantiate all stars at current pos in the starPlaceRadius
        stars = new GameObject[starCount];
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = Instantiate(starGO, starParent.transform);
            star.transform.localPosition = currentPos + new Vector2(Random.Range(-starPlaceRadius, starPlaceRadius), Random.Range(-starPlaceRadius, starPlaceRadius));

            stars[i] = star;
        }
        
        StartCoroutine(CycleStars());
        // LOADING ANIMATION
    }
    private IEnumerator CycleStars()
    {
        print("START CYCLE STARS");
        float timeCounter = Time.realtimeSinceStartup;

        // delete last star and move all stars one index down then place new star at the end
        while (true)
        {
            print("CYCLE STARS");
            if (Time.realtimeSinceStartup - timeCounter >= starMinCycleTime)
            {
                timeCounter = Time.realtimeSinceStartup;

                // translate the currentpos vector 2 into an angle
                float angle = Mathf.Atan2(currentPos.y, currentPos.x) * Mathf.Rad2Deg;
                angle += moveAmountPerFrame;
                currentPos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * starMoveRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * starMoveRadius);

                // replace starsReplacedByFrame stars
                for (int i = 0; i < starsReplacedPerFrame; i++)
                {
                    GameObject lastStar = stars[starCount - 1];
                    for (int j = starCount - 1; j > 0; j--)
                    {
                        stars[j] = stars[j - 1];
                    }

                    stars[0] = lastStar;
                    lastStar.transform.localPosition = currentPos + new Vector2(Random.Range(-starPlaceRadius, starPlaceRadius), Random.Range(-starPlaceRadius, starPlaceRadius));
                }

                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
    }

    public void LoadScene(string startScene, string tarScene)
    {
        // start loading coroutine
        StartCoroutine(LoadSceneCoroutine(startScene, tarScene));
    }

    public IEnumerator LoadSceneCoroutine(string startScene, string tarScene)
    {
        // fade to black
        float startTime = Time.realtimeSinceStartup;
        float duration = fadeDuration;

        blackScreen.gameObject.SetActive(true);

        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = elapsed / duration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // unload start scene and load target scene
        SceneManager.UnloadSceneAsync(startScene);
        SceneManager.LoadScene(tarScene, LoadSceneMode.Additive);

        // fade to transparent
        startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = 1 - elapsed / duration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackScreen.gameObject.SetActive(false);
    }

    public void LoadSceneWithLoadingScreen(string startScene, string tarScene)
    {
        // start loading coroutine
        StartCoroutine(LoadingScreenCoroutine(startScene, tarScene));
    }

    private IEnumerator LoadingScreenCoroutine(string startScene, string tarScene)
    {
        if (tarScene == "Game")
        {
            // set game starting to true
            MapDiscoveryManager.gameStarting = true;
        }

        // fade to black
        float startTime = Time.realtimeSinceStartup;
        float duration = fadeDuration;

        blackScreen.gameObject.SetActive(true);

        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = elapsed / duration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // display loading screen and camera
        loadingCamera.enabled = true;
        loadingScreen.enabled = true;

        // unload start scene and load target scene
        SceneManager.UnloadSceneAsync(startScene);
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);

        while (!loadingScene.isDone)
        {
            print("loading scene progress: " + loadingScene.progress);
            yield return null;
        }

        while (MapDiscoveryManager.gameStarting)
        {
            yield return null;
        }

        print("NOW HIDING LOADING SCREEN");

        // hide loading screen and camera
        loadingCamera.enabled = false;
        loadingScreen.enabled = false;

        // fade to transparent
        startTime = Time.realtimeSinceStartup;
        
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            float alpha = 1 - elapsed / duration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackScreen.gameObject.SetActive(false);
    }
}
