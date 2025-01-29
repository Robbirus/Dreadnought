using Unity.VisualScripting;
using UnityEngine;

public class GenerateCollectible : MonoBehaviour
{
    [SerializeField]
    private GameObject collectibles;
    [Range(1.0f, 2000.0f)]
    public float spawnRange;
    public float spawnTimer;
    private void Start()
    {
        InvokeRepeating(nameof(GenerateHealthCollectibles), 2.0f, spawnTimer);
    }

    private void GenerateHealthCollectibles()
    {
        Vector3 position = new Vector3(Random.Range(0f, spawnRange), 4, Random.Range(0f, spawnRange));
        GameObject NewCollectible = Instantiate(collectibles, position, transform.rotation);
    }
}
