using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject skillMenu;
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseBtn.SetActive(false);
        pauseMenu.SetActive(true);
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    public void SkillMenu()
    {
        skillMenu.SetActive(true);
    }
}
