using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject skillMenu;

    private GameObject mainCamera;
    private GameObject mapCamera;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject mapCanvas;
    [SerializeField] private GameObject quitConfirmation;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        mapCamera = GameObject.Find("MapCamera");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseBtn.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ShowMap()
    {
        // move map camera to player position
        mapCamera.transform.position = mainCamera.transform.position;

        // switch to map camera
        mainCamera.GetComponent<Camera>().enabled = false;
        mapCamera.GetComponent<Camera>().enabled = true;



        // switch to map canvas
        mainCanvas.GetComponent<Canvas>().enabled = false;
        mapCanvas.GetComponent<Canvas>().enabled = true;

        print("map canvas active");
    }

    public void HideMap()
    {
        // switch to main camera
        mainCamera.GetComponent<Camera>().enabled = true;
        mapCamera.GetComponent<Camera>().enabled = false;

        // switch to main canvas
        mainCanvas.GetComponent<Canvas>().enabled = true;
        mapCanvas.GetComponent<Canvas>().enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseBtn.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void BackToPreGame()
    {
        Time.timeScale = 1;

        GetComponent<DataPersistanceManager>().SaveGame();

        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    public void ShowQuitConfirmation()
    {
        quitConfirmation.SetActive(true);
    }

    public void HideQuitConfirmation()
    {
        quitConfirmation.SetActive(false);
    }

    public void SkillMenu()
    {
        skillMenu.SetActive(true);
        transform.GetComponent<SkillMenuManager>().AvailableSkillList();
    }
}
