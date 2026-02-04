using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    private int enemyCount;

    [SerializeField] private GameConfigSO config;

    private List<EnemyController> enemies = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }

    private void Update()
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ManagedUpdate();
        }
    }

    public bool CanSpawn()
    {
        return enemyCount < config.enemyLimit;
    }
}
