using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuButtons : MonoBehaviour
{
    public void Play()
    {
        PersistentSceneManager.instance.LoadScene("StartMenu", "CharacterSelection");
    }

    public void Settings()
    {
        PersistentSceneManager.instance.LoadScene("StartMenu", "Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
