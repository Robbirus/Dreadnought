using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask whoCanCollect = LayerMaskHelper.CreateLayerMask(9);

    private Collectable collectable;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();    
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (LayerMaskHelper.ObjInLayerMask(collider.gameObject, whoCanCollect))
        {
            collectable.Collect(collider.gameObject);

            Destroy(gameObject);
        }
        
    }
}
