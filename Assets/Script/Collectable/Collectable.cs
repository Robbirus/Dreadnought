using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO collectable;

    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void Collect(GameObject objectCollected)
    {
        collectable.Collect(objectCollected);
    }
}
