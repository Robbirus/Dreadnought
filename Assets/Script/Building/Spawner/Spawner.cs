using UnityEngine;
using UnityEngine.Rendering;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Properties")]
    [Tooltip("The differents types of enemies")]
    [SerializeField] private EnemySO[] enemyTypes;

    [Header("Spawner Properties")]
    [Tooltip("The minimum time an enemy can spawn")]
    [SerializeField] private float minimumSpawnTime = 10f;
    [Tooltip("The maximum time an enemy can spawn")]
    [SerializeField] private float maximumSpawnTime = 30f;

    private float timeUntilSpawn;

    private void Awake()
    {
        SetTimeUntilSpawn();
    }

    private void Start()
    {
        gameObject.GetComponent<SpawnerSoundManager>().PlayFactory();
    }

    private void Update()
    {
        if(GameManager.instance.GetEnemyCount() < GameManager.ENEMY_LIMIT)
        {
            SpawnAnEnemy();
        }
    }

    private void SpawnAnEnemy()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < minimumSpawnTime)
        {
            gameObject.GetComponent<SpawnerSoundManager>().PlaySpawnSound();

            EnemySO type = enemyTypes[Random.Range(0, enemyTypes.Length)];
            InstantiateEnemy(type);
            GameManager.instance.IncreaseEnemyCount();
            SetTimeUntilSpawn();
        }
    }
    private void InstantiateEnemy(EnemySO type)
    {
        GameObject enemy = Instantiate(type.enemyPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        enemy.GetComponent<EnemyController>().Setup(type);
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

}
