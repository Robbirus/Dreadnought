using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    [Header("Health Properties")]
    [Tooltip("Max player's life")]
    [SerializeField] private float maxHealth = 1950f;
    [Tooltip("Current player's life")]
    [SerializeField] private float health;
    [Tooltip("Base player's armor")]
    [SerializeField] private int armor = 222;

    // Events
    public event Action<float, float> OnHealthChanged;

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
        NotifyUI();
    }

    private void NotifyUI()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug Key heal / damage
        if (Input.GetKey(KeyCode.X))
        {
            TakeDamage(new DamageInfo
            {
                damage = UnityEngine.Random.Range(15, 110),
                penetration = 0,
                isCrit = false,
                sourceTeam = Team.Enemy
            });
        }
        if (Input.GetKey(KeyCode.V))
        {
            RestoreHealth(UnityEngine.Random.Range(15, 110));
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        this.health -= damageInfo.damage;
        NotifyUI();
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
        NotifyUI();
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

    public float GetHealth()
    {
        return this.health;
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
