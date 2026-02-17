using System;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Image frontExperienceBar;
    private PlayerExperienceManager xpManager;

    private void Start()
    {
        xpManager = GameManager.instance.GetPlayerController().GetXpManager();

        xpManager.OnExperienceChanged += UpdateBar;
        xpManager.OnLevelUp += OnLevelUp;
    }

    private void OnDestroy()
    {
        xpManager.OnExperienceChanged -= UpdateBar;
        xpManager.OnLevelUp -= OnLevelUp;
    }

    private void UpdateBar(float progress)
    {
        frontExperienceBar.fillAmount = progress;
    }
    
    private void OnLevelUp(int newLevel)
    {
        frontExperienceBar.fillAmount = 0f;
    }
}
