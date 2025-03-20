using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private int pity = 0;

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
            HitByShell(collider);
        }
    }

    private void ContactWithPlayer(GameObject collider)
    {
        float damage = gameObject.GetComponent<EnemyController>().GetDamage();
        collider.transform.GetComponent<PlayerHealthManager>().TakeDamage(damage);

        GameManager.instance.enemyKilled++;
        Destroy(gameObject);
    }

    private void HitByShell(GameObject collider)
    {
        float lifeSteal = 0;
        float damage;

        int crit = collider.gameObject.GetComponent<Shell>().GetCritChance();
        int randomValue = Random.Range(1, 100 - pity);
        Debug.Log("random " + randomValue + " : crit " + crit + " : pity " + pity);
        if (Random.Range(1, 100 - pity) <= collider.gameObject.GetComponent<Shell>().GetCritChance())
        {
            damage = collider.gameObject.GetComponent<Shell>().GetDamage() * collider.gameObject.GetComponent<Shell>().GetCritCoef();
            lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * damage;
            gameObject.GetComponent<EnemyHealthManager>().TakeDamage(damage);
            pity = 0;
        }
        else
        {
            damage = collider.gameObject.GetComponent<Shell>().GetDamage(); 
            lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * damage;
            gameObject.GetComponent<EnemyHealthManager>().TakeDamage(collider.gameObject.GetComponent<Shell>().GetDamage());
            pity++;
        }

        ApplyLifeRip(lifeSteal);
        Destroy(collider.gameObject);
    }

    private void ApplyLifeRip(float lifeSteal)
    {
        if (GameManager.instance.GetPlayerHealthManager().IsLifeRipObtained()) 
        {
            GameManager.instance.GetPlayerHealthManager().RestoreHealth(lifeSteal);
        }
        
    }
}
