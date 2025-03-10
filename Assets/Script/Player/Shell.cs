using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header("Shell Attribut")]
    [SerializeField]
    private float shellSpeed = 200;
    [SerializeField]
    private float lifeTime = 10f;

    private float damage;
    private int critChance;
    private float critCoef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = GameManager.instance.GetGunManager().GetDamage();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = gameObject.transform.forward * shellSpeed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Enemies to take damage
        if(collision.transform.CompareTag("Ennemy"))
        {
            if (Random.Range(1, 100) == critChance)
            {
                collision.transform.GetComponent<EnemyHealthManager>().TakeDamage(damage * critCoef);
            }
            else
            {
                collision.transform.GetComponent<EnemyHealthManager>().TakeDamage(damage);
            }
        }

        Destroy(gameObject); // Destroy shell in all cases
    }
}
