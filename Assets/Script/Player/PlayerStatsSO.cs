using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Data/stats")]
public class PlayerStatsSO : ScriptableObject
{
    public float maxHealth;
    public int armor;
    public float maxSpeed;
    public float rotationSpeed;
}
