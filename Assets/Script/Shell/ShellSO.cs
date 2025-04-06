using UnityEngine;

[CreateAssetMenu(fileName = "New Shell", menuName = "Shell Type Systems/Shell")]
public class ShellSO : ScriptableObject
{
    public ShellType ShellType;
    public int velocity;
    public float damage;
    public int penetration;
    public float lifeTime;
}