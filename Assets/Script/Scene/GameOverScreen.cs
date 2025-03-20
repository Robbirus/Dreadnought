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
        GameManager.instance.SetPlayerFound(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby");
        GameManager.instance.SetPlayerFound(false);
    }
}
