using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int ENEMY_LIMIT = 100;
    private int enemyCount = 0;

    #region Player Script
    private PlayerController playerController;
    private GameObject player;
    #endregion

    #region player Attributes
    public int score = 0;
    private bool isPlayerFound = false;
    #endregion

    private GameState currentState;

    #region Player Stats
    private int enemyKilled = 0;
    private int shot = 0;
    private int penetrativeShot = 0;
    private int nonePenetrativeShot = 0;
    private int damageBlocked = 0;
    #endregion

    public static GameManager instance = null;
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.Menu);   
    }

    private void FixedUpdate()
    {
        if (!isPlayerFound)
        {
            InitiatePlayer();
        }
        else
        {
            score = playerController.GetXpManager().GetExperience();
        }
    }

    /// <summary>
    /// Try to find the player with his tag, when found get all script component.
    /// Then stop searching for player
    /// </summary>
    public void InitiatePlayer()
    {
        if (player != null)
        {
            // Get a reference of all player script
            playerController = player.GetComponent<PlayerController>();

            score = playerController.GetXpManager().GetExperience();

            isPlayerFound = true;
        }
        else
        {
            player = GameObject.FindWithTag("Player");
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

        if (UpgradeManager.instance != null)
        {
            UpgradeManager.instance.HidePerkSelection();
        }
    }
   
    private void ApplyMenu()
    {
        MusicManager.instance.PlayMenuMusic();
    }

    private void ApplyGameOver()
    {
        isPlayerFound = false;
        MusicManager.instance.PlayGameOverMusic();
        SceneManager.LoadScene((int)SceneIndex.GAME_OVER);
    }

    private void ApplyPerkSelection()
    {
        UpgradeManager.instance.ShowPerkSelection();
    }
    #endregion

    #region Getter / Setter
    public int GetScore()
    {
        return score;
    }

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

    public int GetEnemyKilled()
    {
        return this.enemyKilled;
    }

    public int GetEnemyCount()
    {
        return this.enemyCount;
    }

    public void IncreaseEnemyKilled()
    {
        this.enemyKilled++;
    }

    public void IncreaseEnemyCount()
    {
        this.enemyCount++;
    }

    public void DecreaseEnemyCount()
    {
        this.enemyCount--;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public void SetPlayerFound(bool isPlayerFound)
    {
        this.isPlayerFound = isPlayerFound;
    }

    public void IncreaseShot()
    {
        this.shot++;
    }

    public void IncreasePenetrativeShot()
    {
        this.penetrativeShot++;
    }

    public void IncreaseNonPenetrativeShot()
    {
        this.nonePenetrativeShot++;
    }

    public void AddDamageBlocked(int damageBlocked)
    {
        this.damageBlocked += damageBlocked;
    }

    public int GetAllShotFired()
    {
        return this.penetrativeShot + this.nonePenetrativeShot;
    }

    public int GetShotFired()
    {
        return this.shot;
    }
    public int GetNonePenetratingShot()
    {
        return this.nonePenetrativeShot;
    }

    public int GetPenetratingShot()
    {
        return this.penetrativeShot;
    }
    #endregion

    public enum GameState
    {
        Menu,
        Playing,
        GameOver,
        PerkSelection
    }
}
