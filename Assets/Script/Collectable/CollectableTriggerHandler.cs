using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask whoCanCollect;

    private void OnTriggerEnter(Collider collider)
    {
        if (LayerMaskHelper.ObjInLayerMask(collider.gameObject, whoCanCollect))
        {

        }
        
    }
}
