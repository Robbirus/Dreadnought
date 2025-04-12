using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    [Header("Enemy Health Stat")]
    [SerializeField] private int maxHealth = 1700;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= (damage - armor);
        healthBar.UpdateHealthBar(health, maxHealth);
        gameObject.GetComponent<EnemySoundManager>().PlayHitSound();

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

            if (GameManager.instance.GetPlayerHealthManager().GetBloodbathObtained())
            {
                StartCoroutine(Bloodbath());
            }

            Destroy(gameObject);
        }
    }

    private void GenerateDrop()
    {
        foreach (GameObject drop in drops)
        {
            Instantiate(drop, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
        }
    }

    IEnumerator Bloodbath()
    {
        float timer = 4f;

        do
        {
            timer -= Time.deltaTime;
            GameManager.instance.GetPlayerHealthManager().RestoreHealth(90f);

            yield return null;
        }
        while (timer > 0);
    }

    #region Getter / Setter
    public int GetArmor()
    {
        return this.armor;
    }

    public void SetMaxHealth(int health)
    {
        this.maxHealth = health;
    }
    #endregion
}
