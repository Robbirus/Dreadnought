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
        collider.transform.GetComponent<PlayerHealthManager>().TakeDamage(damage);

        GameManager.instance.IncreaseEnemyKilled();
        Destroy(gameObject);
    }

    /*
    private void HitByShell(GameObject collider)
    {
        float lifeSteal = 0;
        float damage = 0;
        int penetration = collider.gameObject.GetComponent<Shell>().GetPenetration();
        int pity = collider.gameObject.GetComponent<Shell>().GetPity();
        int armor = gameObject.GetComponent<EnemyHealthManager>().GetArmor();

        if (!CanPenetrate(armor, penetration))
        {
            Destroy(collider.gameObject);
            return;
        }

        if (Random.Range(1, 100 - pity) <= collider.gameObject.GetComponent<Shell>().GetCritChance())
        {
            // Get the damage time the crits coef
            damage = collider.gameObject.GetComponent<Shell>().GetDamage() * collider.gameObject.GetComponent<Shell>().GetCritCoef();

            // Get the life steal proportion
            lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * damage;

            // Apply the damage to the enemy
            gameObject.GetComponent<EnemyHealthManager>().TakeDamage(damage);

            // Reset the pity when a crit happened
            collider.gameObject.GetComponent<Shell>().ResetPity();
        }
        else
        {
            // Get the damage 
            damage = collider.gameObject.GetComponent<Shell>().GetDamage();

            // Get the life steal proportion
            lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * damage;

            // Apply the damage to the enemy
            gameObject.GetComponent<EnemyHealthManager>().TakeDamage(collider.gameObject.GetComponent<Shell>().GetDamage());

            // Reset the pity when a crit happened
            collider.gameObject.GetComponent<Shell>().IncreasePity();
        }

        // Apply the lifeSteal
        ApplyLifeRip(lifeSteal);

        // Destroy the shell
        Destroy(collider.gameObject);
    }
    */


    /// <summary>
    /// Check if LifeSteal Perk is obtained, if true, restore Health to the player equal of the life stolen
    /// if false, do nothing
    /// </summary>
    /// <param name="lifeSteal">The value of the life stole from the enemy</param>
    private void ApplyLifeRip(float lifeSteal)
    {
        if (GameManager.instance.GetPlayerController().GetHealthManager().IsLifeRipObtained()) 
        {
            GameManager.instance.GetPlayerController().GetHealthManager().RestoreHealth(lifeSteal);
        }
        
    }
}
