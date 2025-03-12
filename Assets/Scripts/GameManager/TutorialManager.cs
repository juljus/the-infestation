using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IDataPersistance
{
    private bool tutorialComplete = false;

    private void StartTutorial()
    {
    }

    public void EndTutorial()
    {
        tutorialComplete = true;
    }

    //! IDataPersistance
    public void LoadData(GameData data)
    {
        this.tutorialComplete = data.tutorialComplete;

        if (!tutorialComplete)
        {
            StartTutorial();
        }
    }

    public void SaveData(ref GameData data)
    {
        data.tutorialComplete = this.tutorialComplete;
    }

    public void InGameSave(ref GameData data)
    {
    }
}
