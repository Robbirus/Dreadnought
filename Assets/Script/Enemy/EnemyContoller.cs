using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("Player Instance")]
    [SerializeField] private GameObject player;

    private Rigidbody rigibidbody;
    private Vector3 direction;

    [Header("Ennemy Stats")]
    [SerializeField] private float ennemySpeed = 10f;
    // [SerializeField] private float maxSpeed = 40f;
    [SerializeField] private int damage;

    private void Awake()
    {
        rigibidbody = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        direction = transform.forward;
        damage = UnityEngine.Random.Range(105, 120);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        transform.Translate(0, 0, ennemySpeed * Time.fixedDeltaTime);
        Quaternion orientation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = orientation;
    }

    #region Getter Setter
    public float GetDamage()
    {
        return damage;
    }
    #endregion
}
