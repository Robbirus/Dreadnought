using UnityEngine;

public class LayerMaskHelper : MonoBehaviour
{
    /// <summary>
    /// Return true if the gameobject's layer is contained within the layermask's layers.
    /// </summary> 
    /// <param name="gameObject"> The GameObject we are comparing against the LayerMask. </param>
    /// <param name="layerMask"> The LayerMask we are checking if the GameObject is within. </param>
    /// <return></return>
    
    public static bool ObjInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        if ((layerMask.value & (gameObject.layer)) > 0)
        {
            return true;
        }
        return false;
    }

    public static LayerMask CreateLayerMask(params int[] layers)
    {
        LayerMask layerMask = 0;
        foreach (int layer in layers)
        {
            layerMask |= layer;
        }

        return layerMask;   
    }
}
