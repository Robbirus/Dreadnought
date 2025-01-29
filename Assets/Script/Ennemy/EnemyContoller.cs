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
    private int xp;
    [SerializeField]
    private int damage;
    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rigibidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        direction = transform.forward;
        damage = UnityEngine.Random.Range(5, 20);
        xp = UnityEngine.Random.Range(20, 80);
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

    private float CalculateAccelerate(float currentSpeed)
    {
        // Si la vitesse maximale est atteinte, l'acceleration est nulle
        if (currentSpeed >= maxSpeed)
        {
            return 0;
        }

        // Calcul de l'acceleration 
        return (maxSpeed - currentSpeed) / 2f;
    }

    private void TankForwardBackward()
    {
        // Obtention de la vitesse actuel
        this.currentSpeed = rigibidbody.linearVelocity.magnitude;

        // calcul de l'acceleration
        float acceleration = CalculateAccelerate(this.currentSpeed);

        if (direction != Vector3.zero) 
        {
            // Application de la force dans la direction specifiee
            Vector3 force = direction * 1 * acceleration * rigibidbody.mass;
            rigibidbody.AddForce(force);

            // Limitation de la vitesse
            if (rigibidbody.linearVelocity.magnitude > maxSpeed)
            {
                rigibidbody.linearVelocity = rigibidbody.linearVelocity * maxSpeed;
            }
        } 
        else
        {
            Debug.LogWarning("Vector is null, no force applied");
        }
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

    public void death()
    {
        player.transform.GetComponent<ExperienceManager>().GainExperience(xp);
        Destroy(gameObject);
    }

}
