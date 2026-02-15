using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceManager : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private int baseXP = 100;
    [SerializeField] private int linearXP = 100;
    [Space(10)]

    private int currentLevel = 0;
    private int totalExperience = 0;
    private int bonus = 0;

    // Events
    public event Action<float> OnExperienceChanged;
    public event Action<int> OnLevelUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // UpdateLevel();
        NotifyUI();
    }

    public void GainExperience(int xpAmount)
    {
        totalExperience += xpAmount + bonus;
        CheckLevelUp();
        NotifyUI();
    }

    private void CheckLevelUp()
    {
        bool isLeveledUp = false;

        while(totalExperience > GetXPForLevel(currentLevel + 1))
        {
            currentLevel++;
            isLeveledUp = true;
            OnLevelUp?.Invoke(currentLevel);
        }

        if (isLeveledUp)
        {
            GameManager.instance.ChangeState(GameState.PerkSelection);
        }
    }

    /// <summary>
    /// Use a quadratic formula to compute xp for a specific level
    /// </summary>
    /// <param name="level">The level number used to compute xp amout</param>
    /// <returns>The amount of mandatory xp for this level</returns>
    private int GetXPForLevel(int level)
    {
        return baseXP * level * level + linearXP * level;
    }

    private void NotifyUI()
    {
        // This level
        int currentLevelXP = GetXPForLevel(currentLevel);
        // The next level
        int nextLevelXP = GetXPForLevel(currentLevel + 1);

        float progress = 
            (float)(totalExperience - currentLevelXP) / 
            (nextLevelXP - currentLevelXP);

        OnExperienceChanged?.Invoke(progress);
    }

    /*
    public void GainExperience(int xpAmount)
    {
        totalExperience += xpAmount + bonus;
        CheckChange();
    }
    
    private void CheckChange()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentlevel++;
            UpdateLevel();

            // Level Up Scene
            GameManager.instance.ChangeState(GameState.PerkSelection);
        }
        else
        {
            UpdateExpBar();
        }
    }

    private void UpdateExpBar()
    {
        float expFraction = (float)totalExperience / (float)nextLevelsExperience;
        float fillFront = frontExperienceBar.fillAmount;
        frontExperienceBar.fillAmount = expFraction;
    }

    private void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentlevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentlevel + 1);
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        frontExperienceBar.fillAmount = 0;
    }
*/

    #region Getter / Setter
    public int GetCurrentLevel()
    {
        return this.currentLevel;
    }

    public int GetBonus()
    {
        return this.bonus;
    }

    public void SetBonus(int bonus)
    {
        this.bonus = bonus;
    }
    #endregion
}