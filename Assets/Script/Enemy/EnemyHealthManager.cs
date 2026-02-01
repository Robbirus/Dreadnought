using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, IDamageable
{

    [Header("Enemy Health Stat")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int armor = 5;
    [SerializeField] private HealthBar healthBar;
    [Space(10)]

    [Header("Drop List")]
    [SerializeField] List<GameObject> drops;

    private float health;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void HandleHit(Shell shell, RaycastHit hit)
    {
        int penetration = shell.GetPenetration();
        float damage = shell.GetFinalDamage();

        float finalDamage;

        // Armor
        if(penetration > armor)
        {
            finalDamage = damage;
        }
        else
        {
            finalDamage = 1;
        }

        bool isCrit = shell.GetOwner() == Team.Player && damage > shell.GetFinalDamage() * 0.99f;
        TakeDamage(new DamageInfo
        {
            damage = finalDamage,
            isCrit = false
        });
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        health -= damageInfo.damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (damageInfo.isCrit)
        {
            gameObject.GetComponent<EnemySoundManager>().PlayHitCritSound();
        }
        else
        {
            gameObject.GetComponent<EnemySoundManager>().PlayHitSound();
        }

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            gameObject.GetComponent<EnemySoundManager>().PlayDeathSound();
            GameManager.instance.DecreaseEnemyCount();
            GameManager.instance.IncreaseEnemyKilled();

            GenerateDrop();

            if (GameManager.instance.GetPlayerController().GetHealthManager().GetBloodbathObtained())
            {
                StartCoroutine(Bloodbath());
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Generates items this enemy may drop
    /// </summary>
    private void GenerateDrop()
    {
        foreach (GameObject drop in drops)
        {
            Instantiate(drop, gameObject.transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
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
    public void SetMaxHealth(int health)
    {
        this.maxHealth = health;
    }
    #endregion
}
