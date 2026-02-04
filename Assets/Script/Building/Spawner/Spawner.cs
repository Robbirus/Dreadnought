using UnityEngine;

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
        if (!EnemyManager.instance.CanSpawn()) return;

        gameObject.GetComponent<SpawnerSoundManager>().PlaySpawnSound();

        EnemySO type = enemyTypes[Random.Range(0, enemyTypes.Length)];
        InstantiateEnemy(type);
    }

    private void InstantiateEnemy(EnemySO type)
    {
        GameObject enemyGO = Instantiate(type.enemyPrefab, transform.position, Quaternion.identity);
        
        EnemyController enemy = enemyGO.GetComponent<EnemyController>();

        enemy.Setup(type, GameManager.instance.GetPlayerController());
    }
}
