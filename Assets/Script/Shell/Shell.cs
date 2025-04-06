using System;
using UnityEditor;
using UnityEngine;

public class Shell : MonoBehaviour
{    
    private float damage;
    private int critChance;
    private float critCoef;
    private int pity;
    private int penetration;
    private int velocity;

    private Vector3 direction;

    private void Start()
    {
        direction = transform.forward;

        critChance = GameManager.instance.GetGunManager().GetCritChance();
        critCoef = GameManager.instance.GetGunManager().GetCritCoef();
        pity = GameManager.instance.GetGunManager().GetPity();
    }

    public void Setup(ShellSO currentShell)
    {
        int damageVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;
        int penetrationVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;

        penetration = currentShell.penetration * penetrationVariation;
        damage = currentShell.damage * damageVariation;
        velocity = currentShell.velocity;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(currentShell.color, 0f), new GradientColorKey(Color.white, 1f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );

        gameObject.GetComponent<TrailRenderer>().colorGradient = gradient;
    }

    private void FixedUpdate()
    {
        transform.position += direction * velocity * Time.fixedDeltaTime;
    }

    #region Getter / Setter
    public void SetPenetration(int penetration)
    {
        this.penetration = penetration;
    }

    public int GetPenetration()
    {
        return this.penetration;
    }

    public int GetCritChance()
    {
        return critChance;
    }

    public void SetCritChance(int critChance)
    {
        this.critChance = critChance;
    }

    public int GetPity()
    {
        return this.pity;
    }
    public void ResetPity()
    {
        GameManager.instance.GetGunManager().ResetPity();
    }

    public void IncreasePity()
    {
        GameManager.instance.GetGunManager().IncreasePity();
    }

    public float GetCritCoef()
    {
        return critCoef;
    }

    public void SetCritCoef(int critCoef)
    {
        this.critCoef = critCoef;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    #endregion
}
