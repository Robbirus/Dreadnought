using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    [Header("Level Loader Script")]
    [SerializeField] private LoadingController loadingController;
    [Space(5)]
    
    [Header("Volume Setting")]
    [SerializeField] private AudioMixer audioMixer;
    // [SerializeField] private TMP_Text masterTextValue = null;
    // [SerializeField] private Slider masterSlider = null;
    // [SerializeField] private float defaultMaster = 0.7f;
    
    [Space(5)]

    [SerializeField] private TMP_Text bgmTextValue = null;
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private float defaultBGM = 0.5f;

    [Space(5)]

    [SerializeField] private TMP_Text sfxTextValue = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private float defaultSFX = 0.5f;

    private float bgmVolume = 0.7f;
    private float sfxVolume = 0.5f;

    [Header("Gameplay Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;

    [Header("Confirmation Image")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to Load")]
    [SerializeField] private string newGameLevel;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;

    private string levelToLoad;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    #region Dialog Methods
    public void NewGameDialogYes()
    {
        loadingController.ApplyGame();
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            loadingController.ApplyGame();
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public void ResetButton(string MenuType)
    {
        switch (MenuType)
        {
            case "Audio":
                /*
                AudioListener.volume = defaultMaster;
                audioMixer.SetFloat("Master", Mathf.Log10(defaultBGM) * 20);
                masterSlider.value = defaultMaster;
                masterTextValue.text = defaultMaster.ToString("0.0");
                */

                audioMixer.SetFloat("BGM", Mathf.Log10(defaultBGM) * 20);
                bgmSlider.value = defaultBGM;
                bgmTextValue.text = defaultBGM.ToString("0.0");

                audioMixer.SetFloat("SFX", Mathf.Log10(defaultSFX) * 20);
                sfxSlider.value = defaultSFX;
                sfxTextValue.text = defaultSFX.ToString("0.0");


                ApplyVolume();
                break;

            case "Gameplay":
                controllerSenTextValue.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSen = defaultSen;
                invertYToggle.isOn = false;
                ApplyGameplay();
                break;

            case "Graphics":
                // Reset brightness value
                brightnessSlider.value = defaultBrightness;
                brightnessTextValue.text = defaultBrightness.ToString("0.0");

                // Reset quality value
                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);

                // Reset fullscreen 
                fullScreenToggle.isOn = false;
                Screen.fullScreen = false;

                // Reset Resolution
                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropDown.value = resolutions.Length;
                ApplyGraphics();
                break;

        }
    }
    #endregion

    #region Audio Setting Methods
    public void SetMaster()
    {
        /*
        AudioListener.volume = volume;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        masterTextValue.text = volume.ToString("0.0");
        */
    }
    public void SetBGM()
    {
        bgmVolume = bgmSlider.value;

        //AudioListener.volume = volume;
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);
        bgmTextValue.text = bgmVolume.ToString("0.0");
    }
    public void SetSFX()
    {
        sfxVolume = sfxSlider.value;

        //AudioListener.volume = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        sfxTextValue.text = sfxVolume.ToString("0.0");
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        // Show Prompt
        StartCoroutine(ConfirmationBox());
    }
    #endregion

    #region Gameplay Methods
    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void ApplyGameplay()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("InvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 0);
        }

        PlayerPrefs.SetFloat("Sensitivity", mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        this.brightnessLevel = brightness;
        this.brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        this.qualityLevel = qualityIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeResolution()
    {
        SetResolution(resolutionDropDown.value);
    }

    /// <summary>
    /// Apply the graphics settings, with brightness, quality and/or fullscreen
    /// </summary>
    public void ApplyGraphics()
    {
        // Change your brightness with your post processing or whatever it is
        PlayerPrefs.SetFloat("Brightness", brightnessLevel);

        PlayerPrefs.SetInt("Quality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("Fullscreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox());
    }
    #endregion

    /// <summary>
    /// Show an image to confirm action
    /// </summary>
    /// <returns></returns>
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
