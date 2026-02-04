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

    private int shotFired = 0;
    private int penetrativeShot = 0;
    private int nonePenetrativeShot = 0;
    private int allContactsShots = 0;
    private float precision = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        this.shotFired = GameManager.instance.GetShotFired();
        this.penetrativeShot = GameManager.instance.GetPenetrativeShot();
        this.nonePenetrativeShot = GameManager.instance.GetNonePenetrativeShot();

        this.allContactsShots = this.nonePenetrativeShot + this.penetrativeShot;

        if (this.shotFired > 0) this.precision = (float)this.allContactsShots / this.shotFired;

        scoreTotal.text = "Score : " + GameManager.instance.GetScore() + " points.";
        enemyKilled.text = "Enemy Killed : " + GameManager.instance.GetEnemyKilled();
        accuracy.text = "Accuracy : " + (this.precision * 100f).ToString("0.0") + " %";
        shot.text = "Shots fired : " + this.shotFired;
        penetratingShot.text = "Penetrating shots : " + this.penetrativeShot;
        nonPenetratingShot.text = "Non penetrating shots : " + this.nonePenetrativeShot;

        PlayerController player = GameManager.instance.GetPlayerController();
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
