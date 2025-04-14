using System;
using UnityEngine;

[ExecuteAlways]
[RequireComponent (typeof(BoxCollider))]
public class HitZone : MonoBehaviour
{

    [Header("Armor stats")]
    [Tooltip("Zone Name")]
    public string zoneName = "Zone";
    [Tooltip("Nominal armor thickness")]
    public int armorThickness = 100;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = GetColorByArmor(armorThickness);

        Collider collider = GetComponent<Collider>();
        if(collider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(collider.bounds.center - transform.position, collider.bounds.size);
            Gizmos.DrawCube(collider.bounds.center - transform.position, collider.bounds.size * 0.99f);
        }

        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, $"{zoneName} ({armorThickness}mm)");
    }

    private Color GetColorByArmor(float armorThickness)
    {
        // Thin Armor
        if(armorThickness < 50f)
        {
            return new Color(1f, 0.2f, 0.2f, 0.3f);
        }
        // Mid Armor
        if (armorThickness < 120f)
        {
            return new Color(1f, 1f, 0.2f, 0.3f);
        }
        // Heavy Armor
        return new Color(0.2f, 1f, 0.2f, 0.3f);
    }
#endif
}
