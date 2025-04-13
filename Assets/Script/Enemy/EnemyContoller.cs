using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("Player Instance")]
    [SerializeField] private GameObject player;

    [Header("Script instance")]
    [Tooltip("Health Manager")]
    [SerializeField] private EnemyHealthManager healthManager;

    private Rigidbody rigibidbody;
    private Vector3 direction;

    private string enemyName;
    private float moveSpeed;
    private float rotationSpeed;
    private int maxHealth;
    private int damage;

    private float radius;
    private float height;
    private float baseOffset;

    private void Awake()
    {
        rigibidbody = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        direction = transform.forward;
    }

    public void Setup(EnemySO enemySO)
    {
        enemyName = enemySO.enemyName;
        moveSpeed = enemySO.moveSpeed;
        rotationSpeed = enemySO.rotationSpeed;
        maxHealth = enemySO.maxHealth;
        damage = enemySO.damage;
        radius = enemySO.radius;
        height = enemySO.height;
        baseOffset = enemySO.baseOffset;

        healthManager.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // CheckMovement();
    }

    private void CheckMovement()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        transform.Translate(0, 0, 10 * moveSpeed * Time.fixedDeltaTime);
        Quaternion orientation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = orientation;
    }

    #region Getter Setter
    public float GetDamage()
    {
        return damage;
    }

    public float GetSpeed()
    {
        return this.moveSpeed;
    }

    public float GetAngularSpeed()
    {
        return this.rotationSpeed;
    }

    public GameObject GetPlayer()
    {
        return this.player;
    }

    public EnemyHealthManager GetHealthManager()
    {
        return healthManager;
    }

    public float GetHeight()
    {
        return this.height;
    }

    public float GetRadius()
    {
        return this.radius;
    }

    public float GetBaseOffset()
    {
        return this.baseOffset;
    }
    #endregion
}
