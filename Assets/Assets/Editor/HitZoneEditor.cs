using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitZone))]
public class HitZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HitZone zone = (HitZone)target;

        EditorGUILayout.LabelField("Armor parameters", EditorStyles.boldLabel);

        zone.zoneName = EditorGUILayout.TextField("Zone's name", zone.zoneName);
        zone.armorThickness = (int)EditorGUILayout.Slider("Armor (mm)", zone.armorThickness, 0, 300);

        EditorGUILayout.Space();

        if(zone.armorThickness < 50f)
        {
            EditorGUILayout.HelpBox("Zone with thin armor", MessageType.Info);
        }
        else if(zone.armorThickness > 150f)
        {
            EditorGUILayout.HelpBox("Zone with heavy armor", MessageType.Warning);
        }

        if (GUI.changed) 
        {
            EditorUtility.SetDirty(zone);
        }
    }
}
