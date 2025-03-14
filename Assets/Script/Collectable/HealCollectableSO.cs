using UnityEngine;

[CreateAssetMenu(fileName = "HealCollectable", menuName = "Collectable/HealCollectable")]
public class HealCollectable : CollectableSO
{
    [Header("Heal Attributes")]
    public float healAmount = 50f;

    public override void Collect(GameObject objectThatCollected)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthManager>().RestoreHealth(healAmount);
    }
}
