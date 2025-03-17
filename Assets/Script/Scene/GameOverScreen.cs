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
            score.text = "You've scored : " + GameManager.instance.GetScore() + " points.";
            score.text += "\n";
            score.text += "You've killed : " + GameManager.instance.enemyKilled + " enemies.";
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
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
