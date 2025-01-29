using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField]
    private AnimationCurve experienceCurve;

    [Header("Experience Bar UI")]
    [SerializeField]
    private Image frontExperienceBar;

    private int currentlevel;
    private int totalExperience;
    private int previousLevelsExperience;
    private int nextLevelsExperience;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateLevel();
    }

    public void GainExperience(int xpAmount)
    {
        totalExperience += xpAmount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentlevel++;
            UpdateLevel();
        }
    }

    private void UpdateLevel()
    {
        previousLevelsExperience = (int) experienceCurve.Evaluate(currentlevel);
        nextLevelsExperience = (int) experienceCurve.Evaluate(currentlevel + 1);
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;
        frontExperienceBar.fillAmount = (float)start / (float)end;
    }

}
