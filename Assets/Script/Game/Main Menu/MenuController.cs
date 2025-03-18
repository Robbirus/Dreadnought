using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Text masterTextValue = null;
    [SerializeField] private Slider masterSlider = null;
    [SerializeField] private float defaultMaster = 0.7f;

    [SerializeField] private TMP_Text bgmTextValue = null;
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private float defaultBGM = 0.5f;

    [SerializeField] private TMP_Text sfxTextValue = null;
    [SerializeField] private Slider sfxSlider = null;
    [SerializeField] private float defaultSFX = 0.5f;

    [Header("Gameplay Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Confirmation Image")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to Load")]
    [SerializeField] private string newGameLevel;
    [SerializeField] private GameObject noSavedGameDialog = null;

    private string levelToLoad;

    private float bgmVolume = 0.7f;
    private float sfxVolume = 0.5f;

    private void Start()
    {
        
    }

    #region Dialog Methods
    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
        GameManager.instance.SetIsPlayerAlive(true);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
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
            PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        }

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
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
