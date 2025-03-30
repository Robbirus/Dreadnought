using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class LoadPrefs : MonoBehaviour 
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;
    [Space(10)]

    [Header("Volume Setting")]
    // Audio Mixer
    [SerializeField] private AudioMixer audioMixer;
    [Space(5)]

    // Master Volume
    // [SerializeField] private TMP_Text masterVolumeTextValue;
    // [SerializeField] private Slider masterVolumeSlider = null;
    // [Space(5)]

    // BGM Volume
    [SerializeField] private TMP_Text bgmVolumeTextValue;
    [SerializeField] private Slider bgmVolumeSlider = null;
    [Space(5)]

    // SFX Volume
    [SerializeField] private TMP_Text sfxVolumeTextValue;
    [SerializeField] private Slider sfxVolumeSlider = null;
    [Space(10)]

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [Space(10)]

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdrown;
    [Space(10)]

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;
    [Space(10)]

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [Space(10)]

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse) 
        {
            #region Load Volume
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("BGMVolume");

                bgmVolumeTextValue.text = localVolume.ToString("0.0");
                bgmVolumeSlider.value = localVolume;
                audioMixer.SetFloat("BGM", Mathf.Log10(localVolume) * 20);
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("SFXVolume");

                sfxVolumeTextValue.text = localVolume.ToString("0.0");
                sfxVolumeSlider.value = localVolume;
                audioMixer.SetFloat("SFX", Mathf.Log10(localVolume) * 20);
            }
            else
            {
                menuController.ResetButton("Audio");
            }
            #endregion

            #region Load Graphics
            if (PlayerPrefs.HasKey("Quality"))
            {
                int localQuality = PlayerPrefs.GetInt("Quality");
                qualityDropdrown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if(PlayerPrefs.HasKey("Fullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("Fullscreen");
                
                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if(PlayerPrefs.HasKey("Brightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("Brightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }
            #endregion

            #region Load Gameplay
            if (PlayerPrefs.HasKey("Sensitivity"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("Sensitivity");

                controllerSenTextValue.text = localSensitivity.ToString("0");
                controllerSenSlider.value = localSensitivity;
                menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if(PlayerPrefs.HasKey("InvertY"))
            {
                if(PlayerPrefs.GetInt("InvertY") == 1)
                {
                    invertYToggle.isOn = true;
                } 
                else
                {
                    invertYToggle.isOn = false;
                }
            }
            #endregion
        }
    }
}
