using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int ENEMY_LIMIT = 100;
    private int enemyCount = 0;

    private PlayerController playerController;
    private PerkSelectionUI perkSelectionUI;

    public int score = 0;

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
        Debug.Log("STATE : " + newState);
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


}
