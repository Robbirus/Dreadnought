using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, bool isCrit);

    void HandleHit(Shell shell, RaycastHit hit);
}
