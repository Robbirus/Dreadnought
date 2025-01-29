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

    public static ExperienceManager instance = null;

    private int currentlevel;
    private int totalExperience;
    private int previousLevelsExperience;
    private int nextLevelsExperience;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateLevel();
    }

    public void GainExperience(int xpAmount)
    {
        totalExperience += xpAmount;
        CheckChange();
    }

    private void CheckChange()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentlevel++;
            UpdateLevel();

            // Level Up Scene
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
        previousLevelsExperience = (int) experienceCurve.Evaluate(currentlevel);
        nextLevelsExperience = (int) experienceCurve.Evaluate(currentlevel + 1);
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        frontExperienceBar.fillAmount = 0;
    }

    public int GetExperience()
    {
        return this.totalExperience;
    }
}
