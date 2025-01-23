using System.Collections;
using UnityEngine;

public class SpawnerOverTime : MonoBehaviour
{
    [SerializeField]
    private float timePeriod = 1.0f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject spawner;

    private bool isNewSpawnRequest;
    private Coroutine spawnCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnCoroutine = StartCoroutine(CreateSpawnerCoroutine());
    }

    private IEnumerator CreateSpawnerCoroutine()
    {

        while (true) 
        {
            CreateSpawner();
            yield return new WaitForSeconds(timePeriod);
        }

    }

    public void RequestNewSpawner()
    {
        isNewSpawnRequest = true;
    }
    public void StopSpawnning()
    {
        if (spawnCoroutine == null) return;
        
        StopCoroutine(spawnCoroutine);
    }

    private void CreateSpawner()
    {
        GameObject spawnerInstance = Instantiate(spawner, transform);
        Spawner ennemiesSpawner = spawnerInstance.GetComponent<Spawner>();
        ennemiesSpawner.SetPlayer(player);
    }

}
