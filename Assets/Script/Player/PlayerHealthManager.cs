using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    [Header("Health Properties")]
    [Tooltip("Max player's life")]
    [SerializeField] private float maxHealth = 1950f;
    [Tooltip("Current player's life")]
    [SerializeField] private float health;
    [Tooltip("Base player's armor")]
    [SerializeField] private int armor = 222;

    [Header("GameObject Player")]
    [SerializeField] private float chipSpeed = 2f;

    [Header("Health Bar UI")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;

    private float lerpTimer;

    #region BloodBath
    [SerializeField] private bool bloodbathObtained = false;
    #endregion

    #region Life Rip
    [SerializeField] private float lifeRip = 0;
    [SerializeField] private bool lifeRipObtained = false;
    #endregion

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
            TakeDamage(new DamageInfo
            {
                damage = Random.Range(15, 110),
                penetration = 0,
                isCrit = false,
                sourceTeam = Team.Enemy
            });
        }
        if (Input.GetKey(KeyCode.V))
        {
            RestoreHealth(Random.Range(15, 110));
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

    public void TakeDamage(DamageInfo damageInfo)
    {
        this.health -= damageInfo.damage;
        UpdateHealthUI();
        CheckDeath();
    }

    public void HandleHit(Shell shell, RaycastHit hit)
    {
        int penetration = shell.GetPenetration();
        float baseDamage = shell.GetFinalDamage();

        float finalDamage;

        // Armor
        if (penetration > armor)
        {
            finalDamage = baseDamage;
        }
        else
        {
            finalDamage = 1f;
        }

        TakeDamage(new DamageInfo
        {
            damage = finalDamage,
            isCrit = shell.IsCrit(),
            sourceTeam = shell.GetOwner()
        });
    }

    private void CheckDeath()
    {
        if(health <= 0)
        {
            GameManager.instance.ChangeState(GameState.GameOver);
        }
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    #region Getter / Setter
    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    public void SetBloodbathObtained(bool bloodbathObtained)
    {
        this.bloodbathObtained = bloodbathObtained;
    }

    public bool GetBloodbathObtained()
    {
        return this.bloodbathObtained;
    }

    public void SetLifeRip(float lifeRip)
    {
        this.lifeRip = lifeRip;
    }

    public float GetLifeRip()
    {
        return this.lifeRip;
    }

    public void SetLifeRipObtained(bool lifeRipObtained)
    {
        this.lifeRipObtained = lifeRipObtained;
    }

    public bool IsLifeRipObtained()
    {
        return this.lifeRipObtained;
    }

    public int GetArmor()
    {
        return this.armor;
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
    }
    #endregion
}
