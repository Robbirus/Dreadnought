using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("Player Instance")]
    [SerializeField]
    private GameObject player;

    private Rigidbody rigibidbody;
    private Vector3 direction;

    [Header("Ennemy Stats")]
    [SerializeField]
    private float ennemySpeed = 10f;
    [SerializeField]
    private float maxSpeed = 40f;
    [SerializeField]
    private int damage;

    private float currentSpeed;

    private void Awake()
    {
        rigibidbody = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        direction = transform.forward;
        damage = UnityEngine.Random.Range(5, 20);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        transform.Translate(0, 0, ennemySpeed * Time.deltaTime);
        Quaternion orientation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = orientation;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Transform hitTransform = collider.transform;
        if(hitTransform.CompareTag("Player"))
        {
            hitTransform.GetComponent<PlayerHealthManager>().TakeDamage(damage);

            // Destroy the ennemy when it touches the player
            Destroy(gameObject);

        }
    }
}
