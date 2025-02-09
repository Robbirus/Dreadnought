using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.musicSource.clip = AudioManager.instance.menuMusic;
        AudioManager.instance.musicSource.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
