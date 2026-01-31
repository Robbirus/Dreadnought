using System;
using UnityEngine;

public class Shell : MonoBehaviour
{    
    private float damage;
    private int critChance;
    private float critCoef;
    private bool isCrit = false;
    private int pity;
    private int penetration;
    private int velocity;
    private float lifeTime;

    private Vector3 direction;
    private float lifeSteal;
    private int caliber;
    private ShellType type;

    private Team owner;

    private void Start()
    {
        direction = transform.forward;

        direction.Normalize();

        critChance  = GameManager.instance.GetPlayerController().GetGunManager().GetCritChance();
        critCoef    = GameManager.instance.GetPlayerController().GetGunManager().GetCritCoef();
        pity        = GameManager.instance.GetPlayerController().GetGunManager().GetPity();

        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// Instantiate a shell depending of the type in the SO
    /// </summary>
    /// <param name="currentShell">The SO shell used this time</param>
    public void Setup(ShellSO currentShell, Team team, int caliber, bool isCrit)
    {
        float damageVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;
        float penetrationVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;

        penetration = (int)(currentShell.penetration * penetrationVariation);
        damage      = currentShell.damage * damageVariation;
        velocity    = currentShell.velocity;
        lifeTime    = currentShell.lifeTime;
        type        = currentShell.ShellType;

        this.caliber = caliber;
        this.isCrit = isCrit;

        owner = team;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(currentShell.color, 0f), new GradientColorKey(Color.white, 1f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );

        gameObject.GetComponent<TrailRenderer>().colorGradient = gradient;
    }

    private void FixedUpdate()
    {        
        Vector3 nextPos = transform.position + direction * velocity * Time.deltaTime;
        Vector3 move = nextPos - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, move.magnitude))
        {
            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

            if (damageable != null) 
            {
                damageable.HandleHit(this, hit);
                Destroy(gameObject);
                return;
            }
        }

        transform.position = nextPos;
    }

    /// <summary>
    /// Check if LifeSteal Perk is obtained, if true, restore Health to the player equal of the life stolen
    /// if false, do nothing
    /// </summary>
    /// <param name="lifeSteal">The value of the life stole from the enemy</param>
    private void ApplyLifeRip(float lifeSteal)
    {
        if (GameManager.instance.GetPlayerController().GetHealthManager().IsLifeRipObtained())
        {
            GameManager.instance.GetPlayerController().GetHealthManager().RestoreHealth(lifeSteal);
        }
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
        GameManager.instance.GetPlayerController().GetGunManager().ResetPity();
    }

    public void IncreasePity()
    {
        GameManager.instance.GetPlayerController().GetGunManager().IncreasePity();
    }

    public float GetCritCoef()
    {
        return critCoef;
    }

    public void SetCritCoef(int critCoef)
    {
        this.critCoef = critCoef;
    }

    public float GetFinalDamage()
    {
        if(this.owner == Team.Player)
        {
            float finalDamage = isCrit ? damage * critCoef : damage;

            if(isCrit)
            {
                ResetPity();
            }
            else
            {
                IncreasePity();
            }

            return finalDamage;
        }

        return damage;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
  
    public Team GetOwner()
    {
        return owner;
    }
    #endregion
}
