using System.Collections;
using UnityEngine;

namespace CodeGraph.UnityCourse.Enemies.CharacterController
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform player;
        [SerializeField]
        private GameObject enemy;
        [SerializeField]
        private float spawnFrequencySeconds = 5f;
        [SerializeField]
        private Transform spawnSphere;

        private void Start()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            // Be careful with infinite loop: make sure to yield and wait
            while (true)
            {
                yield return new WaitForSeconds(spawnFrequencySeconds);
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            GameObject newEnemy = Instantiate(enemy, transform);
            Vector2 spawnPosition2D = Random.insideUnitCircle;
            Vector3 spawnPosition3D = new Vector3
            {
                x = spawnPosition2D.x * spawnSphere.lossyScale.x * 0.5f,
                y = 0,
                z = spawnPosition2D.y * spawnSphere.lossyScale.z * 0.5f
            };
            newEnemy.transform.position = spawnSphere.position + spawnPosition3D;
            EnemyController controller = newEnemy.GetComponent<EnemyController>();
            controller.SetTarget(player);
        }
    }
}
