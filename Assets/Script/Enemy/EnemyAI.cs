using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{ 
    [SerializeField] private Transform turret;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private ShellSO currentShell;

    [Header("Script instances")]
    [SerializeField] private EnemyController enemyController;

    private NavMeshAgent agent;
    private PlayerController player;
    private EnemyState currentState;
    private float lastShootTime;

    private int caliber;
    private float damage;
    private int penetration;
    private float attackRange;
    private float attackCooldown;

    
    public void Init(PlayerController player, EnemyController enemy)
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = enemyController.GetSpeed() * 1000;
        agent.angularSpeed = enemyController.GetAngularSpeed() * 100;

        agent.baseOffset = enemyController.GetBaseOffset();
        agent.radius = enemyController.GetRadius();
        agent.height = enemyController.GetHeight();

        this.caliber = enemyController.GetCaliber();
        this.damage = enemyController.GetDamage();
        this.penetration = enemyController.GetPenetration();
        this.attackRange = enemyController.GetAttackRange();
        this.attackCooldown = enemyController.GetAttackCooldown();

        this.player = player;
        this.enemyController = enemy;
        currentState = EnemyState.Chase;
    }

    public void CallUpdate()
    {
        if (this.player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

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

        Vector3 direction = (player.transform.position + new Vector3(0f, 2.5f, -2f)) - turret.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
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
        agent.SetDestination(player.transform.position);

        if (distance <= attackRange + 0.5f)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    private void Shoot()
    {
        //InstantiateShell();

        GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.position, shellSpawnPoint.rotation);
        shell.GetComponent<Shell>().Setup(
            currentShell,
            Team.Enemy,
            enemyController.GetCaliber(),
            false,
            enemyController.GetPenetration(),
            enemyController.GetDamage()
        );
    }

    private void InstantiateShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shell.GetComponent<Shell>().Setup(currentShell, Team.Enemy, this.caliber, false, this.penetration, this.damage);
    }
}

public enum EnemyState
{
    Chase,
    Attack
}
