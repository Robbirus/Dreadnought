using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Player Script
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private PlayerHealthManager healthManager;
    private ExperienceManager xpManager;
    private GunManager gunManager;
    private GameObject player;
    #endregion

    #region player Attributes
    public int score = 0;
    private bool isPlayerFound = false;
    #endregion

    private GameState currentState;

    #region Enemy numbers
    [Header("Enemy stats")]
    public const int ENEMY_LIMIT = 100;
    private int enemyCount = 0;
    private int enemyKilled = 0;
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
            score = xpManager.GetExperience();
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
            xpManager = player.GetComponent<ExperienceManager>();
            healthManager = player.GetComponent<PlayerHealthManager>();
            gunManager = player.GetComponent<GunManager>();
            playerController = player.GetComponent<PlayerController>();
            playerMovement = player.GetComponent<PlayerMovement>();

            score = xpManager.GetExperience();

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
        if (xpManager == null)
        {
            return 0;
        }
        else
        {
            return xpManager.GetCurrentLevel();
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

    public PlayerHealthManager GetPlayerHealthManager()
    {
        return healthManager;
    }

    public GunManager GetGunManager()
    {
        return gunManager;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }
    public PlayerMovement GetPlayerMovement()
    {
        return this.playerMovement;
    }

    public ExperienceManager GetExperienceManager()
    {
        return xpManager;
    }

    public void SetPlayerFound(bool isPlayerFound)
    {
        this.isPlayerFound = isPlayerFound;
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
