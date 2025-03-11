using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField]
    private float maxHealth = 1950f;
    [SerializeField]
    private int armor = 10;

    [Header("GameObject Player")]
    [SerializeField]
    private float chipSpeed = 2f;

    [Header("Health Bar UI")]
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;

    private float health;
    private float lerpTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth/2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // UI
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Debug Key heal / damage
        if (Input.GetKey(KeyCode.X))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKey(KeyCode.V))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage - armor;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void SetHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }
    public void SetArmor(int armor)
    {
        this.armor = armor;
    }

    public int GetArmor()
    {
        return this.armor;
    }
}
