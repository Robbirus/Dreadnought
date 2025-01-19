using UnityEngine;

namespace CodeGraph.UnityCourse.Enemies.CharacterController
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Transform player;

        [SerializeField]
        private float speed = 1f;

        private void Update()
        {
            if (player == null) return;
            Vector3 enemyToPlayer = player.position - transform.position;
            transform.position += enemyToPlayer.normalized * speed * Time.deltaTime;
            transform.LookAt(player.position);
        }

        public void SetTarget(Transform target)
        {
            player = target;
        }
    }
}
