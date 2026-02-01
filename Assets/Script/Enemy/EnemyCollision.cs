using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        ProcessCollision(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void ProcessCollision(GameObject collider)
    {
        // If the enemy touches the player
        if (collider.transform.CompareTag("Player") && gameObject != null)
        {
            ContactWithPlayer(collider);
        }

        // if a shell touches the enemy
        if (collider.transform.CompareTag("Shell") && gameObject != null)
        {
            // HitByShell(collider);
        }
    }

    private void ContactWithPlayer(GameObject collider)
    {
        float damage = gameObject.GetComponent<EnemyController>().GetDamage();
        collider.transform.GetComponent<PlayerHealthManager>().TakeDamage(new DamageInfo
        {
            damage = damage,
            isCrit = false
        });

        GameManager.instance.IncreaseEnemyKilled();
        Destroy(gameObject);
    }
}
