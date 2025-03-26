using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [Header("Static Pause Value")]
    public static bool isGamePaused = false;
    [Space(10)]

    [Header("Pause menu panel")]
    [SerializeField] private GameObject pauseMenuUI;

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
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.ChangeState(GameManager.GameState.Menu);
        Destroy(GameObject.FindWithTag("Player"));
        SceneManager.LoadScene((int)SceneIndex.MENU);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        Time.timeScale = 1f;
    }
}
