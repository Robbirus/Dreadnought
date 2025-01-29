using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header("Shell Attribut")]
    [SerializeField]
    private float shellSpeed = 200;
    [SerializeField]
    private float lifeTime = 5f;

    private float damage = 110f;
    private float crits = 2f;

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
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Ennemy"))
        {
            try
            {
                hitTransform.GetComponent<EnemyController>().death();
            }            
            catch 
            {
                Debug.Log("Not an ennemy");
            }
            Destroy(gameObject);
        }
    }
}
