using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);

    void HandleHit(Shell shell, RaycastHit hit);
}

public struct DamageInfo
{
    public float damage;
    public int penetration;
    public bool isCrit;
    public Team sourceTeam;
}
