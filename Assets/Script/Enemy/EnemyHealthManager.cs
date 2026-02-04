using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyHealthManager : MonoBehaviour, IDamageable
{
    [Header("Enemy Health Stat")]
    [SerializeField] private HealthBar healthBar;
    [Space(10)]

    [Header("Drop List")]
    [SerializeField] List<GameObject> drops;

    public float health;
    public float maxHealth;
    public int armor;
    private EnemySoundManager soundManager;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        soundManager = gameObject.GetComponent<EnemySoundManager>(); 
    }

    public void Setup(int maxHealth, int armor)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

        this.armor = armor;
    }

    /// <summary>
    /// When hit, compute penetration and armor value
    /// compute life steal
    /// Then apply damage
    /// </summary>
    /// <param name="shell">The shell that hit this enemy</param>
    /// <param name="hit">Where does the shell hit</param>
    public void HandleHit(Shell shell, RaycastHit hit)
    {
        int penetration = shell.GetPenetration();
        float baseDamage = shell.GetFinalDamage();

        float finalDamage;

        // Armor Check
        if (penetration > this.armor)
        {
            finalDamage = baseDamage;
            GameManager.instance.IncreasePenetrativeShot();
        }
        else
        {
            finalDamage = 1;
            GameManager.instance.IncreaseNonePenetrativeShot();
        }

        bool isCrit = shell.IsCrit();

        // Apply Damage
        TakeDamage(new DamageInfo
        {
            damage = finalDamage,
            isCrit = isCrit,
            sourceTeam = shell.GetOwner()
        });

        // Apply Liferip from player
        if (shell.GetOwner() == Team.Player)
        {
            PlayerHealthManager playerHealth = GameManager.instance
                .GetPlayerController()
                .GetHealthManager();

            if (playerHealth.IsLifeRipObtained())
            {
                float heal = finalDamage * playerHealth.GetLifeRip();
                playerHealth.RestoreHealth(heal);
            }
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        health -= damageInfo.damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (damageInfo.isCrit)
        {
            soundManager.PlayHitCritSound();
        }
        else
        {
            soundManager.PlayHitSound();
        }

        CheckDeath();
    }

    /// <summary>
    /// Verify if this enemy should be dead,
    /// if true, generates drops and activate bloodbath
    /// if obtained
    /// </summary>
    private void CheckDeath()
    {
        if (health <= 0)
        {
            soundManager.PlayDeathSound();

            GenerateDrop();

            if (GameManager.instance.GetPlayerController().GetHealthManager().GetBloodbathObtained())
            {
                StartCoroutine(Bloodbath());
            }

            IncreaseEnemyKilled();
            Destroy(gameObject);
        }
    }
    private void IncreaseEnemyKilled()
    {
        GameManager.instance.IncreaseEnemyKilled();
    }

    /// <summary>
    /// Generates items this enemy may drop
    /// </summary>
    private void GenerateDrop()
    {
        foreach (GameObject drop in drops)
        {
            Instantiate(drop, new Vector3(gameObject.transform.position.x, 3f, gameObject.transform.position.z), Quaternion.identity);
        }
    }

    IEnumerator Bloodbath()
    {
        float timer = 4f;

        do
        {
            timer -= Time.deltaTime;
            GameManager.instance.GetPlayerController().GetHealthManager().RestoreHealth(90f);

            yield return null;
        }
        while (timer > 0);
    }

    #region Getter / Setter
    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
    }


    #endregion
}
