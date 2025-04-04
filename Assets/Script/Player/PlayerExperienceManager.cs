using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField]
    private AnimationCurve experienceCurve;

    [Header("Interface")]
    [SerializeField]
    private Image frontExperienceBar;

    private int currentlevel = 0;
    private int totalExperience;
    private int previousLevelsExperience;
    private int nextLevelsExperience;
    private int bonus = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateLevel();
    }

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
            GameManager.instance.ChangeState(GameManager.GameState.PerkSelection);
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

    #region Getter / Setter
    public int GetCurrentLevel()
    {
        return currentlevel;
    }
    public int GetExperience()
    {
        return totalExperience;
    }
    public int GetBonus()
    {
        return bonus;
    }
    public void SetBonus(int bonus)
    {
        this.bonus = bonus;
    }
    #endregion
}
