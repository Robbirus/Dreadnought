using System.Collections;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    [Header("Enemy Health Stat")]
    [SerializeField]
    private int maxHealth = 1700;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private int xp;

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
        xp = UnityEngine.Random.Range(5, 20);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        gameObject.GetComponent<EnemySoundManager>().PlayHitSound();

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            gameObject.GetComponent<EnemySoundManager>().PlayDeathSound();
            GameManager.instance.GetExperienceManager().GainExperience(xp);
            GameManager.instance.enemyCount--;
            GameManager.instance.enemyKilled++;

            if (GameManager.instance.GetPlayerHealthManager().GetBloodbathObtained())
            {
                StartCoroutine(Bloodbath());
            }

            Destroy(gameObject);
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
}
