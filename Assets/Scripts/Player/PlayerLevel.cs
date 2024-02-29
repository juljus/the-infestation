using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLevel : MonoBehaviour, IDataPersistance
{
    private int playerLevel;
    [SerializeField] private TMPro.TMP_Text levelText;

    void Update()
    {
        levelText.text = "Level: " + playerLevel;

        // change level with up/down arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerLevel++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerLevel--;
        }
    }

    public void LoadData(GameData data) {
        this.playerLevel = data.playerLevel;
    }

    public void SaveData(ref GameData data) {
        data.playerLevel = this.playerLevel;
    }
}
