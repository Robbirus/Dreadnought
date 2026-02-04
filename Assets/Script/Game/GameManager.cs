using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    private PerkSelectionUI perkSelectionUI;
    private RunStats currentRunStats;

    private int score = 0;
    private int enemyKilled = 0;
    private int shotFired = 0;
    private int penetrativeShot = 0;
    private int nonePenetrativeShot = 0;

    private GameState currentState;

    public static GameManager instance = null;
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void RegisterPlayer(PlayerController player)
    {
        playerController = player;
    }

    public void UnregisterPlayer(PlayerController player)
    {
        if(playerController == player)
        {
            playerController = null;
        }
    }
    
    public void RegisterPerksUI(PerkSelectionUI perkSelectionUI)
    {
        this.perkSelectionUI = perkSelectionUI;
    }

    public void UnregisterPerksUI(PerkSelectionUI perkSelectionUI)
    {
        if (this.perkSelectionUI == perkSelectionUI)
        {
            this.perkSelectionUI = null;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
        HandleStateChanged();
    }

    private void HandleStateChanged()
    {
        switch (currentState)
        {
            case GameState.Playing:
                ApplyPlaying();
                break;
            case GameState.Menu:
                ApplyMenu();
                break;
            case GameState.Tuto:
                ApplyTuto();
                break;
            case GameState.GameOver:
                ApplyGameOver();
                break;
            case GameState.PerkSelection:
                ApplyPerkSelection();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }

    #region Play State Methods    
    private void ApplyPlaying()
    {
        MusicManager.instance.PlayBackgroundMusic();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentRunStats = new RunStats();

        if (perkSelectionUI == null) return;
        
        perkSelectionUI.Hide();
    }

    private void ApplyMenu()
    {
        MusicManager.instance.PlayMenuMusic();
    }

    private void ApplyTuto()
    {
        throw new NotImplementedException();
    }

    private void ApplyGameOver()
    {
        MusicManager.instance.PlayGameOverMusic();
        SceneManager.LoadScene((int)SceneIndex.GAME_OVER);
    }

    private void ApplyPerkSelection()
    {
        perkSelectionUI.ShowPerks();
        Time.timeScale = 0f; 
        MusicManager.instance.PlayLevelUp();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    #endregion

    #region Getter / Setter
    public int GetCurrentLevel()
    {
        if (playerController.GetXpManager() == null)
        {
            return 0;
        }
        else
        {
            return playerController.GetXpManager().GetCurrentLevel();
        }
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public int GetScore()
    {
        return this.score;
    }

    public void IncreaseEnemyKilled()
    {
        enemyKilled++;
    }

    public int GetEnemyKilled()
    {
        return this.enemyKilled;
    }

    public void IncreaseShotFired()
    {
        this.shotFired++;
    }

    public void IncreasePenetrativeShot()
    {
        this.penetrativeShot++;
    }

    public void IncreaseNonePenetrativeShot()
    {
        this.nonePenetrativeShot++;
    }

    public int GetShotFired()
    {
        return this.shotFired;
    }

    public int GetNonePenetrativeShot()
    {
        return this.nonePenetrativeShot;
    }

    public int GetPenetrativeShot()
    {
        return this.penetrativeShot;
    }
    #endregion
}
