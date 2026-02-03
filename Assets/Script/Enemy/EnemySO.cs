using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy System/new Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy ÅNProperties")]
    public string enemyName;
    public int maxHealth;
    public int caliber;
    public int damage;
    public int penetration;
    [Space(10)]

    [Header("NavMeshAgent Properties")]
    public float baseOffset;
    public float radius;
    public float height;
    public float moveSpeed;
    public float rotationSpeed;
    [Space(10)]

    [Header("Enemy Model")]
    public GameObject enemyPrefab;
}
