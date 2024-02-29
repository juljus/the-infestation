using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuButtons : MonoBehaviour
{

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharSelector");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
