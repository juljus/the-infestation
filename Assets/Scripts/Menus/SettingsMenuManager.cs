using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Button saveButton;

    [SerializeField] private UnityEngine.UI.Slider musicVolumeSlider;
    [SerializeField] private TMPro.TMP_Text musicVolumeValue;
    private float musicVolume;

    [SerializeField] private UnityEngine.UI.Slider sfxVolumeSlider;
    [SerializeField] private TMPro.TMP_Text sfxVolumeValue;
    private float sfxVolume;

    void Start()
    {
        // set the sliders to the saved values
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    void Update()
    {
        // update the values beside sliders
        musicVolumeValue.text = Mathf.Round(musicVolumeSlider.value * 100).ToString();
        sfxVolumeValue.text = Mathf.Round(sfxVolumeSlider.value * 100).ToString();
    }

    public void SaveSettings()
    {
        // save the settings
        musicVolume = musicVolumeSlider.value;
        sfxVolume = sfxVolumeSlider.value;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();
    }

    // data persistance
    public void LoadData(GameData data) {
        this.musicVolume = data.musicVolume;
        this.sfxVolume = data.sfxVolume;
    }
    public void SaveData(ref GameData data) {
        data.musicVolume = this.musicVolume;
        data.sfxVolume = this.sfxVolume;
    }

}
