using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy System/new Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float moveSpeed;
    public float rotationSpeed;
    public int maxHealth;
    public int damage;

    public GameObject enemyPrefab;
}
