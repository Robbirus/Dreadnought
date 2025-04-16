using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Player Instance")]
    [SerializeField] private GameObject player;

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
        caliber = enemySO.caliber;

        healthManager.SetMaxHealth(maxHealth);
    }

    private void OnEnable()
    {
        EnemyManager.instance.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        EnemyManager.instance.UnregisterEnemy(this);
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
