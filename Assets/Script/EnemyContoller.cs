using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Rigidbody rigibidbody;
    private Vector3 direction = Vector3.forward;

    private float ennemySpeed = 10f;
    private float maxSpeed = 40f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rigibidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
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

    private float Accelerate(float currentSpeed)
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
        float currentSpeed = rigibidbody.linearVelocity.magnitude;

        // calcul de l'acceleration
        float acceleration = Accelerate(currentSpeed);

        Vector3 force = direction.normalized * 1 * acceleration * rigibidbody.mass * Time.deltaTime;
        rigibidbody.AddForce(force);

        // Limitation de la vitesse
        if (rigibidbody.linearVelocity.magnitude > maxSpeed)
        {
            rigibidbody.linearVelocity = rigibidbody.linearVelocity.normalized * maxSpeed;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log($"Trigger enter started on gameObject {gameObject.name}");
        if (collision.tag.Equals("Player") || collision.tag.Equals("Shell"))
        {
            Destroy(gameObject);

        }
    }
}
