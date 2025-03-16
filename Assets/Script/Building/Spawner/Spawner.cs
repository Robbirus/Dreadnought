using UnityEngine;
using UnityEngine.Rendering;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float minimumSpawnTime;
    [SerializeField]
    private float maximumSpawnTime;

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
        if(GameManager.instance.enemyCount < GameManager.ENEMY_LIMIT)
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
            Instantiate(enemy, transform.position, Quaternion.identity);
            GameManager.instance.enemyCount++;
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

}
