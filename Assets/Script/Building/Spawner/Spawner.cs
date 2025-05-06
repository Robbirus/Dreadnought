using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Properties")]
    [Tooltip("The differents types of enemies")]
    [SerializeField] private EnemySO[] enemyTypes;

    [Header("Spawn Settings")]
    [Tooltip("Time before the first enemy spawn")]
    [Range(0, 60)] [SerializeField] private float startSpawnTime = 10f;
    [Tooltip("Time between each spawn")]
    [Range(0, 100)] [SerializeField] private float spawnDelay = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), startSpawnTime, spawnDelay);
    }

    private void SpawnEnemy()
    {
        if (GameManager.instance.GetEnemyCount() >= GameManager.ENEMY_LIMIT) return;

        gameObject.GetComponent<SpawnerSoundManager>().PlaySpawnSound();

        EnemySO type = enemyTypes[Random.Range(0, enemyTypes.Length)];
        InstantiateEnemy(type);
        GameManager.instance.IncreaseEnemyCount();
    }

    private void InstantiateEnemy(EnemySO type)
    {
        GameObject enemy = Instantiate(type.enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponentInChildren<EnemyController>().Setup(type);
    }
}
