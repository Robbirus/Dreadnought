using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [Header("Score Stat Text")]
    [SerializeField] private TMP_Text scoreTotal;
    [SerializeField] private TMP_Text enemyKilled;
    [Space(10)]

    [Header("Loading Controller")]
    [SerializeField] private LoadingController loadingController;

    private void Start()
    {        
        if(scoreTotal != null)
        {
            scoreTotal.text = "Score : " + GameManager.instance.score + " points.";
            enemyKilled.text = "Enemy Killed : " + GameManager.instance.enemyKilled;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
    }

    public void RestartGame()
    {
        loadingController.ApplyGame();
        GameManager.instance.SetPlayerFound(false);
    }

    public void ReturnToMenu()
    {
        loadingController.ApplyMenu();
        GameManager.instance.SetPlayerFound(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
