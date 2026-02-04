using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Script instance")]
    [Tooltip("Health Manager")]
    [SerializeField] private EnemyHealthManager healthManager;
    [Tooltip("AI")]
    [SerializeField] private EnemyAI enemyAI;

    private Rigidbody rigibidbody;
    private Vector3 direction;

    private string enemyName;
    private float moveSpeed;
    private float rotationSpeed;
    private int maxHealth;
    private int damage;
    private int caliber;
    private int penetration;
    private float attackRange;
    private float attackCooldown;
    private int armor;

    private float radius;
    private float height;
    private float baseOffset;
    private PlayerController player;

    private void Awake()
    {
        rigibidbody = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        direction = transform.forward;
    }

    public void Setup(EnemySO enemySO, PlayerController player)
    {
        this.player = player;

        enemyName = enemySO.enemyName;
        moveSpeed = enemySO.moveSpeed;
        rotationSpeed = enemySO.rotationSpeed;
        maxHealth = enemySO.maxHealth;
        damage = enemySO.damage;
        radius = enemySO.radius;
        height = enemySO.height;
        baseOffset = enemySO.baseOffset;
        caliber = enemySO.caliber;
        this.penetration = enemySO.penetration;
        this.attackRange = enemySO.attackRange;
        this.attackCooldown = enemySO.attackCooldown;
        this.armor = enemySO.armor;

        healthManager.Setup(enemySO.maxHealth, enemySO.armor);

        enemyAI.Init(this.player, this);
    }

    private void OnEnable()
    {
        EnemyManager.instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        IncreaseEnemyKilled();
        EnemyManager.instance.UnregisterEnemy(this);
    }

    private void IncreaseEnemyKilled()
    {
        GameManager.instance.IncreaseEnemyKilled();
    }

    public void ManagedUpdate()
    {
        enemyAI.CallUpdate();
    }

    #region Getter Setter
    public int GetCaliber()
    {
        return caliber;
    }

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

    public int GetPenetration()
    {
        return this.penetration;
    }

    public PlayerController GetPlayer()
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

    public float GetAttackRange()
    {
        return this.attackRange;
    }

    public float GetAttackCooldown()
    {
        return this.attackCooldown;
    }
    #endregion
}
