using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
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

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            
            loadingSlider.value = progressValue;
            progressTextValue.text = progressValue * 100f + "%";
            yield return null;
        }
    }
}
