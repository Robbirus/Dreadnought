using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Chase,
    Attack
}

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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10 * enemyController.GetSpeed();
        agent.angularSpeed = 100 * enemyController.GetAngularSpeed();

        agent.baseOffset = enemyController.GetBaseOffset();
        agent.radius = enemyController.GetRadius();
        agent.height = enemyController.GetHeight();

        player = enemyController.GetPlayer().transform;
        currentState = EnemyState.Chase;
    }

    private void Update()
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
        shell.GetComponent<Shell>().Setup(currentShell, Team.Enemy, enemyController.GetCaliber());
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
