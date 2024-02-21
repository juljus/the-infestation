using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectorButtons : MonoBehaviour
{

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
