using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GameObject UI Instance")]
    [SerializeField] private GameObject needle;

    #region Player Script
    private PlayerController playerController;
    private PlayerHealthManager healthManager;
    private ExperienceManager xpManager;
    private GunManager gunManager;
    private GameObject player;
    #endregion

    #region Needle attributes
    private float startPosition = 220f;
    private float endPosition = -41f ;
    private float desiredPosition;
    #endregion

    #region player Attributes
    private int score = 0;
    private bool isPlayerAlive = false;
    private bool wasPlayerAlive = false;
    private bool isPlayerFound = false;
    private bool gameOverCalled = false;

    private float tankSpeed;
    private bool invicibleActive = false;
    private float invicibleActiveTime = 0f;
    #endregion

    private GameState currentState;

    #region Enemy numbers
    [Header("Enemy stats")]
    public const int ENEMY_LIMIT = 100;
    public int enemyCount = 0;
    public int enemyKilled = 0;
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

            tankSpeed = playerController.GetCurrentSpeed();
            score = xpManager.GetExperience();

            isPlayerFound = true;
        }
        else
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    private void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = tankSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));   
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

    public ExperienceManager GetExperienceManager()
    {
        return xpManager;
    }

    public void SetIsPlayerAlive(bool alive)
    {
        isPlayerAlive = alive;
    }

    public void SetWasPlayerAlive(bool wasAlive)
    {
        wasPlayerAlive = wasAlive;
    }

    public void SetGameOverCalled(bool methodCalled)
    {
        gameOverCalled = methodCalled;
    }

    public void SetPlayerFound(bool isPlayerFound)
    {
        this.isPlayerFound = isPlayerFound;
    }

    public void SetInvicibleActive(bool invicibleActive)
    {
        this.invicibleActive = invicibleActive;
    }

    public void SetInvicibleActiveTime(float invicibleActiveTime)
    {
        this.invicibleActiveTime = invicibleActiveTime;
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
