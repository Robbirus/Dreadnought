using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Shell", menuName = "Shell Type Systems/Shell")]
public class ShellSO : ScriptableObject
{
    public Color color;
    public ShellType ShellType;
    public Sprite shellImage;
    public int velocity;
    public float damageCoefficient;
    public float penetrationCoefficient;
    public float lifeTime;
}