using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GameObject UI Instance")]
    [SerializeField]
    private GameObject needle;

    #region Player Script
    private TankController tankController;
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

    #region Enemy limit
    public const int ENEMY_LIMIT = 20;
    public int enemyCount = 0;
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
        AudioManager.instance.bgm.clip = AudioManager.instance.menuMusic;
        AudioManager.instance.bgm.Play();
    }

    private void FixedUpdate()
    {
        // Le jeu n'a pas encore commence
        if (!isPlayerAlive && !wasPlayerAlive)
        {
            // Menu
        }

        // Le jeu commence
        if (isPlayerAlive && !wasPlayerAlive)
        {
            // Game
            if(!isPlayerFound)
            {
                AudioManager.instance.bgm.clip = AudioManager.instance.combatMusic;
                AudioManager.instance.bgm.Play();

                try
                {
                    player = GameObject.FindWithTag("Player");
                    xpManager = player.GetComponent<ExperienceManager>();
                    healthManager = player.GetComponent<PlayerHealthManager>();
                    gunManager = player.GetComponent<GunManager>();
                    tankController = player.GetComponent<TankController>();

                    ChangeState(GameState.Playing);

                    isPlayerFound = true;
                }
                catch
                {
                    Debug.Log("Player Not Found");
                }
            }
            Debug.Log("nb enemy : " + enemyCount);
            tankSpeed = tankController.GetCurrentSpeed();
            //UpdateNeedle();
            score = xpManager.GetExperience();
            CheckPlayerAlive();
        }

        // Le jeu est fini
        if (!isPlayerAlive && wasPlayerAlive && !gameOverCalled)
        {
            // Game Over
            gameOverCalled = true;
            SceneManager.LoadScene("Game Over Screen");
            AudioManager.instance.bgm.clip = AudioManager.instance.defeatMusic;
            AudioManager.instance.bgm.Play();
        }
    }

    private void CheckPlayerAlive()
    {
        if(healthManager.GetHealth() <= 0f)
        {
            isPlayerAlive = false;
            wasPlayerAlive = true;
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
                UpgradeManager.instance.HidePerkSelection();
                break;
            case GameState.Title:
                break;
            case GameState.GameOver:
                break;
            case GameState.PerkSelection:
                UpgradeManager.instance.ShowPerkSelection();
                break;
        }
    }

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

    public TankController GetTankController()
    {
        return tankController;
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

    public enum GameState
    {
        Title,
        Menu,
        Playing,
        GameOver,
        PerkSelection
    }
}
