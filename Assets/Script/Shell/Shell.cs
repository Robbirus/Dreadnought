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
    private float ricochetAngle = 70f;
    private float lifeSteal;
    private int caliber;
    private ShellType type;

    private Team owner;

    private void Start()
    {
        direction = transform.forward;

        direction.Normalize();

        critChance = GameManager.instance.GetPlayerController().GetGunManager().GetCritChance();
        critCoef = GameManager.instance.GetPlayerController().GetGunManager().GetCritCoef();
        pity = GameManager.instance.GetPlayerController().GetGunManager().GetPity();
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
        damage = currentShell.damage * damageVariation;
        velocity = currentShell.velocity;
        lifeTime = currentShell.lifeTime;
        type = currentShell.ShellType;
        this.caliber = caliber;
        this.isCrit = isCrit;
        owner = team;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(currentShell.color, 0f), new GradientColorKey(Color.white, 1f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );

        gameObject.GetComponent<TrailRenderer>().colorGradient = gradient;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {        
        Vector3 nextPos = transform.position + direction * velocity * Time.deltaTime;
        Vector3 move = nextPos - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, move.magnitude))
        {

            HitZone zone = hit.collider.GetComponent<HitZone>();

            if (zone != null)
            {
                OnHitTarget(hit, zone);
            }
            else
            {
                KeepShellMoving(nextPos);
            }
        }
        else
        {
            KeepShellMoving(nextPos);
        }
    }

    /// <summary>
    /// Method used to move the shell over time 
    /// </summary>
    /// <param name="nextPos">The next position of the shell</param>
    private void KeepShellMoving(Vector3 nextPos)
    {
        transform.position = nextPos;
    }

    private void OnHitTarget(RaycastHit hit, HitZone zone)
    {
        GameObject impact = hit.collider.transform.parent.gameObject;

        MonoBehaviour target = impact.GetComponent<MonoBehaviour>();
        if (target == null) return;


        if (owner == Team.Player && target is EnemyController enemy)
        {
            HandleHit(enemy, hit, zone, owner);
        }
        else if(owner == Team.Enemy && target is PlayerController player)
        {
            HandleHit(player, hit, zone, owner);
        }
    }

    private void HandleHit(MonoBehaviour target, RaycastHit hit, HitZone zone, Team team)
    {
        float angle = GetAngle(hit, zone);
        ricochetAngle = ObtainRicochetAngle(zone);

        Debug.Log($"Hit : {zone.zoneName}, Angle : {angle}");
        if (angle < ricochetAngle)
        {
            if(CanPenetrate(zone.armorThickness, this.penetration, angle))
            {
                PenetrateTarget(target, team);
            }
            else
            {
                if(team == Team.Player)
                {
                    GameManager.instance.IncreaseNonPenetrativeShot();
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Ricochet !"); 
            Ricochet(hit);
        }
    }

    private void PenetrateTarget(MonoBehaviour target, Team team)
    {
        float finalDamage = damage;
        if (team == Team.Player) 
        {            
            finalDamage = isCrit ? damage * critCoef : damage;

            if (isCrit)
            {
                ResetPity();
            }
            else
            {
                IncreasePity();
            }
        }

        if(target is EnemyController enemy)
        {
            enemy.GetHealthManager().TakeDamage(finalDamage, isCrit);
            ApplyLifeRip(GameManager.instance.GetPlayerController().GetHealthManager().GetLifeRip() * finalDamage);
        }
        else if(target is PlayerController player)
        {
            player.GetHealthManager().TakeDamage(finalDamage);
        }

        if(team == Team.Player)
        {
            GameManager.instance.IncreasePenetrativeShot();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Return the ricochet angle depending of the shell type
    /// </summary>
    /// <param name="zone">The HitZone this shell touched (used for the 3 Calibers rules)</param>
    /// <returns>The ricochet angle</returns>
    private float ObtainRicochetAngle(HitZone zone)
    {
        float ricochet = 0;
        switch (this.type)
        {
            case ShellType.AP:
                ricochet = 70;
                break;
            case ShellType.APCR:
                ricochet = 70;
                break;
            case ShellType.HE:
                ricochet = 90;
                break;
            case ShellType.HEAT:
                ricochet = 85;
                break;
            default:
                ricochet = 70;
                break;
        }

        if(this.type == ShellType.AP || this.type == ShellType.APCR)
        {
            ricochet = Check3CalibersRule(zone, ricochet);
        }

        return ricochet;
    }

    /// <summary>
    /// If the shell calibers is more than thrice the nominal armor thickness, there will be no ricochet
    /// </summary>
    /// <param name="zone">The HitZone this shell touched</param>
    /// <param name="ricochet">The ricochet angle to change</param>
    /// <returns>The ricochet angle</returns>
    private float Check3CalibersRule(HitZone zone, float ricochet)
    {
        if (this.caliber >= 3 * zone.armorThickness)
        {
            ricochet = 0;
        }

        return ricochet;
    }

    /// <summary>
    /// Adjust the angle of impact depending of the shell type
    /// </summary>
    /// <param name="angle">The angle of impact</param>
    /// <returns>The angle normalized</returns>
    private float NormalizeShell(float angle, HitZone zone)
    {
        float normalizationAngle = 0;
        switch (this.type)
        {
            case ShellType.AP:
                normalizationAngle = 5;
                break;
            case ShellType.APCR:
                normalizationAngle = 2;
                break;
            default:
                normalizationAngle = 0;
                break;
        }

        normalizationAngle = Check2CalibersRule(zone, normalizationAngle);

        return angle - normalizationAngle;
    }

    /// <summary>
    /// If the shell calibers is more than twice the nominal armor thickness, this shell gain a little more in normalization
    /// </summary>
    /// <param name="zone">The HitZone this shell touched</param>
    /// <param name="normalizationAngle">The normalization angle by default</param>
    /// <returns>The adjusted normalization angle</returns>
    private float Check2CalibersRule(HitZone zone, float normalizationAngle)
    {
        if (this.caliber >= 2 * zone.armorThickness)
        {
            normalizationAngle = (float)(normalizationAngle * 1.4 * this.caliber / zone.armorThickness);
        }

        return normalizationAngle;
    }

    /// <summary>
    /// Return the angle between the shell and the contact point
    /// </summary>
    /// <param name="hit"></param>
    /// <returns>The angle relative to normal of the collider</returns>
    private float GetAngle(RaycastHit hit, HitZone zone)
    {
        float angle =  Vector3.Angle(-direction, hit.normal);
        return NormalizeShell(angle, zone);
    }

    /// <summary>
    /// Allows a shell to be redirected and lose 25% of penetration value
    /// </summary>
    /// <param name="hit"></param>
    private void Ricochet(RaycastHit hit)
    {
        GameManager.instance.IncreaseNonPenetrativeShot();
        direction = Vector3.Reflect(direction, hit.normal);
        transform.position = hit.point;
        this.penetration *= (75/100);
        
    }

    /// <summary>
    /// Contains of the rules of penetration,
    /// First, compute the relative armor
    /// </summary>
    /// <param name="armor">The enemy nominal armor</param>
    /// <param name="penetration">The penetration value of this shell</param>
    /// <param name="hit"></param>
    /// <returns>True if the enemy can be penetrated</returns>
    private bool CanPenetrate(int armor, int penetration, float angle)
    {
        float relativeArmor = ObtainRelativeArmor(armor, angle);

        return penetration > relativeArmor;
    }

    /// <summary>
    /// Compute the relative armor using the angle and nominal armor
    /// </summary>
    /// <param name="armor">The enemy nominal armor</param>
    /// <param name="hit"></param>
    /// <returns>The relative armor of the enemy</returns>
    private float ObtainRelativeArmor(int armor, float angle)
    {        
        return (armor)/Mathf.Cos(angle);
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
