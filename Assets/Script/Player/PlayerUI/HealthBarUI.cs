using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Health Bar UI")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private float chipSpeed = 2f;

    private PlayerHealthManager healthManager; 
    private float lerpTimer;
    private float targetFill;

    private void Start()
    {
        healthManager = GameManager.instance.GetPlayerController().GetHealthManager();

        healthManager.OnHealthChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        healthManager.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float life, float maxLife)
    {
        targetFill = life / maxLife;
        lerpTimer = 0f;
    }

    private void Update()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;

        if (fillBack > targetFill)
        {
            backHealthBar.color = Color.red;
            frontHealthBar.fillAmount = targetFill;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, targetFill, percentComplete);            
        }

        if (fillFront < targetFill)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = targetFill;

            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }
}
