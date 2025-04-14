using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    [Header("Static Pause Value")]
    public static bool isGamePaused = false;
    [Space(10)]

    [Header("Pause menu panel")]
    [SerializeField] private GameObject pauseMenuUI;
    [Space(10)]

    [Header("Level Loader")]
    [SerializeField] private LevelLoader levelLoader; 
    [Space(10)]
    
    [Header("Volume Setting")]
    [SerializeField] private AudioMixer audioMixer;
    // [SerializeField] private TMP_Text masterTextValue = null;
    // [SerializeField] private Slider masterSlider = null;
    // [SerializeField] private float defaultMaster = 0.7f;

    [Space(5)]

    [SerializeField] private TMP_Text bgmTextValue = null;
    [SerializeField] private Slider bgmSlider = null;

    [Space(5)]

    [SerializeField] private TMP_Text sfxTextValue = null;
    [SerializeField] private Slider sfxSlider = null;
    [Space(10)]

    [Header("Confirmation Image")]
    [SerializeField] private GameObject confirmationPrompt = null;

    private float bgmVolume = 0.7f;
    private float sfxVolume = 0.5f;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused) 
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.ChangeState(GameManager.GameState.Menu);
        Destroy(GameObject.FindWithTag("Player"));
        levelLoader.LoadLevel((int)SceneIndex.MENU);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        Time.timeScale = 1f;
    }

    #region Audio Volume
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
