using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;

    private void Start()
    {        
        score.text = "You've scored : " + ExperienceManager.instance.GetExperience() + " points";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}
