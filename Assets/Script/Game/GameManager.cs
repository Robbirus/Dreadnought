using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GameObject UI Instance")]
    [SerializeField]
    private GameObject needle;

    [Header("Script Instance")]
    [SerializeField]
    private TankController tankController;
    [SerializeField]
    private GunManager gunManager;
    [SerializeField]
    private PlayerHealthManager healthManager;
    [SerializeField]
    private ExperienceManager xpManager;

    private float tankSpeed;

    private float startPosition = 220f;
    private float endPosition = -41f ;
    private float desiredPosition;

    private int score = 0;
    private GameState currentState;

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
        AudioManager.instance.musicSource.clip = AudioManager.instance.combatMusic;
        AudioManager.instance.musicSource.Play();
    }

    private void FixedUpdate()
    {
        if(healthManager.GetHealth() <= 0f)
        {
            score = xpManager.GetExperience();
            SceneManager.LoadScene("Game Over Screen");
        }

        tankSpeed = tankController.GetCurrentSpeed();
        UpdateNeedle();
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
        return xpManager.GetCurrentLevel();
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

    public enum GameState
    {
        Playing,
        PerkSelection
    }
}
