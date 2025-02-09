using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;

    private void Start()
    {
        AudioManager.instance.musicSource.clip = AudioManager.instance.defeatMusic;
        AudioManager.instance.musicSource.Play();
        score.text = "You've scored : " + GameManager.instance.GetScore() + " points";
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
