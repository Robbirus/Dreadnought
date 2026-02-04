using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [Header("Score Stat Text")]
    [SerializeField] private TMP_Text scoreTotal;
    [SerializeField] private TMP_Text enemyKilled;
    [SerializeField] private TMP_Text shot;
    [SerializeField] private TMP_Text penetratingShot;
    [SerializeField] private TMP_Text nonPenetratingShot;
    [SerializeField] private TMP_Text accuracy;
    [Space(10)]

    [Header("Loading Controller")]
    [SerializeField] private LoadingController loadingController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (scoreTotal != null)
        {
            float allShotFired = GameManager.instance.GetAllShotFired();
            float shotsFired = GameManager.instance.GetShotFired();

            float precision = allShotFired / shotsFired;
            if (shotsFired == 0) 
            {
                precision = 0f;
            }
            else
            {
                precision = allShotFired / shotsFired;
            }


                scoreTotal.text = "Score : " + GameManager.instance.score + " points.";
            enemyKilled.text = "Enemy Killed : " + GameManager.instance.GetEnemyKilled();
            accuracy.text = "Accuracy : " + (precision * 100f).ToString("0.0") + " %";
            shot.text = "Shots fired : " + GameManager.instance.GetShotFired();
            penetratingShot.text = "Penetrating shots : " + GameManager.instance.GetPenetratingShot();
            nonPenetratingShot.text = "Non penetrating shots : " + GameManager.instance.GetNonePenetratingShot();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
    }

    public void RestartGame()
    {
        loadingController.ApplyGame();
    }

    public void ReturnToMenu()
    {
        loadingController.ApplyMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
