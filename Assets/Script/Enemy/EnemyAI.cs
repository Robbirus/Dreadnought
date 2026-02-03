using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform shellSpawnPoint;

    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private ShellSO currentShell;

    [Header("Behaviour Properties")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float attackCooldown = 10f;

    [Header("Script instances")]
    [SerializeField] private EnemyController enemyController;

    private EnemyState currentState;
    private float lastShootTime;

    private int caliber;
    private float damage;
    private int penetration;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 1000 * enemyController.GetSpeed();
        agent.angularSpeed = 100 * enemyController.GetAngularSpeed();

        agent.baseOffset = enemyController.GetBaseOffset();
        agent.radius = enemyController.GetRadius();
        agent.height = enemyController.GetHeight();

        player = enemyController.GetPlayer().transform;
        currentState = EnemyState.Chase;

        this.caliber = enemyController.GetCaliber();
        this.damage = enemyController.GetDamage();
        this.penetration = enemyController.GetPenetration();
    }

    public void CallUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Chase:
                ChasingBehaviour(distance);
                break;

            case EnemyState.Attack:
                AttackingBehaviour(distance);
                break;
        }
    }

    private void AttackingBehaviour(float distance)
    {
        agent.speed = 0;
        agent.SetDestination(transform.position);

        Vector3 direction = (player.position + new Vector3(0f, 2.5f, -2f)) - turret.position;
        Quaternion targetRotation  =Quaternion.LookRotation(direction);
        turret.rotation = Quaternion.Lerp(turret.rotation, targetRotation, Time.deltaTime * 5f);

        if (Time.time > lastShootTime + attackCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }

        if (distance > attackRange + 2f)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void ChasingBehaviour(float distance)
    {
        agent.speed = 1000 * enemyController.GetSpeed();
        agent.SetDestination(player.position);
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void Shoot()
    {
        InstantiateShell();
    }

    private void InstantiateShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shell.GetComponent<Shell>().Setup(currentShell, Team.Enemy, this.caliber, false, this.penetration, this.damage);
    }

    #region Getter / Setter
    public Transform GetPlayerTransform()
    {
        return this.player;
    }

    public NavMeshAgent GetAgent()
    {
        return this.agent;
    }

    public float GetAttackRange()
    {
        return this.attackRange;
    }
    #endregion
}

public enum EnemyState
{
    Chase,
    Attack
}
