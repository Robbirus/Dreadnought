using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    [Header("Enemy Health Stat")]
    [SerializeField] private int maxHealth;
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

    public void TakeDamage(float damage, bool isCrit)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (isCrit)
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
