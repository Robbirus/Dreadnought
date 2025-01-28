using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}
