using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Guns/Gun")]
public class GunSO : ScriptableObject
{
    public float damage;
    public float reloadTime;
    public int penetration;
    public int caliber;
    public List<ShellSO> ammo;
}
