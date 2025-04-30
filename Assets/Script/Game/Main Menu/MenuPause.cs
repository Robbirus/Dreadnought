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
    [Space(5)]

    [SerializeField] private TMP_Text bgmTextValue = null;
    [Space(5)]

    [SerializeField] private TMP_Text sfxTextValue = null;
    [Space(10)]

    [Header("Confirmation Image")]
    [SerializeField] private GameObject confirmationPrompt = null;

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
    public void IncreaseBGM()
    {
        if (MenuController.bgmVolume < 10)
        {
            MenuController.bgmVolume++;
        }
        audioMixer.SetFloat("BGM", Mathf.Log10(MenuController.bgmVolume / 10f) * 20);
        bgmTextValue.text = MenuController.bgmVolume.ToString("0");
    }

    public void DecreaseBGM()
    {
        if (MenuController.bgmVolume > 0)
        {
            MenuController.bgmVolume--;
        }

        if (MenuController.bgmVolume == 0)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(-1 * 20));
        }
        else
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(MenuController.bgmVolume / 10f) * 20);
        }

        bgmTextValue.text = MenuController.bgmVolume.ToString("0");
    }

    public void IncreaseSFX()
    {
        if (MenuController.sfxVolume < 10)
        {
            MenuController.sfxVolume++;
        }
        audioMixer.SetFloat("BGM", Mathf.Log10(MenuController.sfxVolume / 10f) * 20);
        sfxTextValue.text = MenuController.sfxVolume.ToString("0");
    }

    public void DecreaseSFX()
    {
        if (MenuController.sfxVolume > 0)
        {
            MenuController.sfxVolume--;
        }

        if (MenuController.sfxVolume == 0)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(-1 * 20));
        }
        else
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(MenuController.sfxVolume / 10f) * 20);
        }

        sfxTextValue.text = MenuController.sfxVolume.ToString("0");
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("BGMVolume", MenuController.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", MenuController.sfxVolume);
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
