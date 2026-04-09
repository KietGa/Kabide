using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private AudioMixer mixer;

    private int gameSpeed;

    private void Start()
    {
        LoadSave();
    }

    private void LoadSave()
    {
        gameSpeed = PlayerPrefs.GetInt("gameSpeed", 1);
        gameSpeedText.text = "Game Speed: " + gameSpeed;

        ChangeMaster(PlayerPrefs.GetFloat("master"));
        ChangeBGM(PlayerPrefs.GetFloat("bgm"));
        ChangeSFX(PlayerPrefs.GetFloat("sfx"));
    }

    public void ChangeGameSpeed()
    {
        gameSpeed++;
        if (gameSpeed > 4) gameSpeed = 1;
        PlayerPrefs.SetInt("gameSpeed", gameSpeed);
        gameSpeedText.text = "Game Speed: " + gameSpeed;
        print(PlayerPrefs.GetInt("gameSpeed", 1));
    }

    public void ChangeBGM(float value)
    {
        bgmSlider.value = value;
        if (value == -40f) value = -80f;
        PlayerPrefs.SetFloat("bgm", value);
        mixer.SetFloat("bgm", value);
    }

    public void ChangeSFX(float value)
    {
        sfxSlider.value = value;
        if (value == -40f) value = -80f;
        PlayerPrefs.SetFloat("sfx", value);
        mixer.SetFloat("sfx", value);
    }

    public void ChangeMaster(float value)
    {
        masterSlider.value = value;
        if (value == -40f) value = -80f;
        PlayerPrefs.SetFloat("master", value);
        mixer.SetFloat("master", value);
    }
}
