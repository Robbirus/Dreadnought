using Unity.VisualScripting;
using UnityEngine;

public class GenerateCollectible : MonoBehaviour
{
    [SerializeField]
    private GameObject collectible;
    [Range(1.0f, 2000.0f)]
    public float spawnRange;
    [Range(1.0f, 120.0f)]
    public float spawnTimer;
    private GameObject newCollectible;
    private bool _canSpawn;

    private void Start()
    {
        SpawnCollectible();
    }

    private void Update()
    {
        Debug.Log(newCollectible);
        Debug.Log(_canSpawn);
        Debug.Log(newCollectible == null && _canSpawn);

        if(newCollectible == null && _canSpawn)
        {
            Invoke("SpawnCollectible", spawnTimer);
            _canSpawn=false;
        }
    }
    private void SpawnCollectible()
    {
        newCollectible = Instantiate(collectible,transform.position + new Vector3(0,4,0), Quaternion.identity);
        _canSpawn = true;
        
    }
}
