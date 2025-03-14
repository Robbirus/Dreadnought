using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;

    private void Start()
    {        
        if(score != null)
        {
            score.text = "You've scored : " + GameManager.instance.GetScore() + " points";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        GameManager.instance.SetIsPlayerAlive(true);
        GameManager.instance.SetWasPlayerAlive(false);
        GameManager.instance.SetGameOverCalled(false);
        GameManager.instance.SetPlayerFound(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby");
        GameManager.instance.SetIsPlayerAlive(false);
        GameManager.instance.SetWasPlayerAlive(false);
        GameManager.instance.SetGameOverCalled(false);
        GameManager.instance.SetPlayerFound(false);
    }
}
