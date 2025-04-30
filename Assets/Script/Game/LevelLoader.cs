using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [Header("Screen")] public GameObject loadingScreen;
    [Header("Slider")] public Slider loadingSlider;
    [Header("Progress")] public TMP_Text progressTextValue;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadLevelASync(sceneIndex));
    }

    IEnumerator LoadLevelASync(int levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        loadingScreen.SetActive(true);

        // If loading Game, Initiate the player stats (not working)
        switch (levelToLoad)
        {
            case (int)SceneIndex.MENU:
                GameManager.instance.ChangeState(GameManager.GameState.Menu);
                break;
            case (int)SceneIndex.GAME:
                GameManager.instance.InitiatePlayer();
                GameManager.instance.ChangeState(GameManager.GameState.Playing);
                break;
            case (int)SceneIndex.TUTO:
                GameManager.instance.InitiatePlayer();
                GameManager.instance.ChangeState(GameManager.GameState.Tuto);
                break;
            case (int)SceneIndex.GAME_OVER:
                GameManager.instance.ChangeState(GameManager.GameState.GameOver);
                break;
        }

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            
            loadingSlider.value = progressValue;
            progressTextValue.text = progressValue * 100f + "%";

            yield return null;
        }

    }
}
