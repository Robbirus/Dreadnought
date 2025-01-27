using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField]
    private float shellSpeed = 200;
    [SerializeField]
    private float lifeTime = 5f;

    private float damage = 110f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = gameObject.transform.forward * shellSpeed;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player")
        {
            try
            {
                collision.gameObject.GetComponent<EnemyController>().death();
            }
            catch
            {
                Debug.Log("Not touching an enemy");
            }
            Destroy(gameObject);
        }
    }
}
