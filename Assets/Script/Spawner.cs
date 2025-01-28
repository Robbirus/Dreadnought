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

    private void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn < minimumSpawnTime)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
        
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

}
