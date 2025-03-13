using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IDataPersistance
{
    private bool tutorialComplete = false;

    [SerializeField] private GameObject[] tutorialOverlays;


    // (0) open in game tutorial overlay
    private void StartTutorial()
    {
        tutorialOverlays[0].SetActive(true);
    }


    // (1) close in game tutorial overlay and open pause menu tutorial overlay (skills)
    public void Tutorial1()
    {
        tutorialOverlays[0].SetActive(false);
        tutorialOverlays[1].SetActive(true);
    }

    // (2) close pause menu tutorial overlay and open skill menu tutorial overlay nr 1
    public void Tutorial2()
    {
        tutorialOverlays[1].SetActive(false);
        tutorialOverlays[2].SetActive(true);
    }

    // (3) close skill menu tutorial overlay and open skill menu tutorial overlay nr 2
    public void Tutorial3()
    {
        print("HIDING NR 2");
        tutorialOverlays[2].SetActive(false);
        print("SHOWING NR 3");
        tutorialOverlays[3].SetActive(true);
    }

    // (4) close skill menu tutorial overlay and open skill menu tutorial overlay nr 3
    public void Tutorial4()
    {
        tutorialOverlays[3].SetActive(false);
        tutorialOverlays[4].SetActive(true);
    }

    // (5) close skill menu tutorial overlay and open skill menu tutorial overlay nr 4
    public void Tutorial5()
    {
        tutorialOverlays[4].SetActive(false);
        tutorialOverlays[5].SetActive(true);
    }

    // (6) close skill menu tutorial overlay and open pause menu tutorial overlay (map)
    public void Tutorial6()
    {
        tutorialOverlays[5].SetActive(false);
        tutorialOverlays[6].SetActive(true);
    }

    // (7) close pause menu tutorial overlay and open map tutorial overlay nr 1
    public void Tutorial7()
    {
        tutorialOverlays[6].SetActive(false);
        tutorialOverlays[7].SetActive(true);
    }

    // (8) close map tutorial overlay and open map tutorial overlay nr 2
    public void Tutorial8()
    {
        tutorialOverlays[7].SetActive(false);
        tutorialOverlays[8].SetActive(true);
    }

    // (9) close map tutorial overlay and open pause menu tutorial overlay (finish)
    public void Tutorial9()
    {
        tutorialOverlays[8].SetActive(false);
        tutorialOverlays[9].SetActive(true);
    }

    // (10) close pause menu tutorial overlay and open in game tutorial overlay (campfire
    public void Tutorial10()
    {
        tutorialOverlays[9].SetActive(false);
        tutorialOverlays[10].SetActive(true);
    }

    // (10) close tutorial overlay and end tutorial
    public void Tutorial11()
    {
        tutorialOverlays[10].SetActive(false);
        EndTutorial();
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
