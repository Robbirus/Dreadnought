using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        GameManager.instance.SetIsPlayerAlive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
