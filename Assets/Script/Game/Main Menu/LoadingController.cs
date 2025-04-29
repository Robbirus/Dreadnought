using UnityEngine;

public class LoadingController : MonoBehaviour
{
    [Header("Level Loader Script")]
    [SerializeField] private LevelLoader levelLoader;

    // Apply Game
    public void ApplyGame()
    {
        levelLoader.LoadLevel((int)SceneIndex.GAME);
    }

    // Apply Menu
    public void ApplyMenu()
    {
        levelLoader.LoadLevel((int)SceneIndex.MENU);
    }    
    
    // Apply Tuto
    public void ApplyTuto()
    {
        levelLoader.LoadLevel((int)SceneIndex.TUTO);
    }

    // Apply Game Over
    public void ApplyGameOver()
    {
        levelLoader.LoadLevel((int)SceneIndex.GAME_OVER);
    }
}
