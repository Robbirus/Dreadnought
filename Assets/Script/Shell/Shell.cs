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
    /// <param name="team">The team where the shell comes from</param>
    /// <param name="caliber">The shell's caliber</param>
    /// <param name="isCrit">True if the shot was a critical hit</param>
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
            if(hit.collider.GetComponentInParent<IDamageable>() is IDamageable target)
            {
                target.HandleHit(this, hit);

                Destroy(gameObject);
            }
        }

        transform.position = nextPos;
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
            return isCrit ? damage * critCoef : damage;
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
   
    public bool IsCrit() 
    { 
        return isCrit; 
    }
    #endregion
}
