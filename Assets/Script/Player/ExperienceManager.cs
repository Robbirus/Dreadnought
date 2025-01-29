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
        CheckChange();
    }

    private void CheckChange()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentlevel++;
            UpdateLevel();
        } 
        else
        {
            UpdateExpBar();
        }
    }

    private void UpdateExpBar()
    {

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

}
