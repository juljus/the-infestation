using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuButtons : MonoBehaviour
{
    public void Play()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
        PersistentSceneManager.instance.LoadScene("StartMenu", "CharacterSelection");
    }

    public void Settings()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
        PersistentSceneManager.instance.LoadScene("StartMenu", "Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
