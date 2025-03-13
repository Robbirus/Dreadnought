using UnityEngine;

[CreateAssetMenu(menuName = "Collectable/Invicible", fileName = "New Invicible Collectable")]
public class CollectableInvicibleSO : CollectableSO
{
    [Header("Collectable Stats")]
    public float activeTime = 24f;

    public override void Collect(GameObject objectThatCollected)
    {
        GameManager.instance.SetInvicibleActive(true);
        GameManager.instance.SetInvicibleActiveTime(activeTime);
    }
}
