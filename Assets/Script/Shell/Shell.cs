using System;
using UnityEngine;

public class Shell : MonoBehaviour
{    
    private float damage;
    private int critChance;
    private float critCoef;
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
    public void Setup(ShellSO currentShell, Team team, int caliber)
    {
        float damageVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;
        float penetrationVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;

        penetration = (int)(currentShell.penetration * penetrationVariation);
        damage = currentShell.damage * damageVariation;
        velocity = currentShell.velocity;
        lifeTime = currentShell.lifeTime;
        type = currentShell.ShellType;
        this.caliber = caliber; 
        owner = team;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(currentShell.color, 0f), new GradientColorKey(Color.white, 1f)},
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );

        gameObject.GetComponent<TrailRenderer>().colorGradient = gradient;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
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

        if(angle < ricochetAngle)
        {
            if(CanPenetrate(zone.armorThickness, this.penetration, angle))
            {
                Debug.Log("Tir pénétrant");
                PenetrateTarget(target, team);
            }
            else
            {
                if(team == Team.Player)
                {
                    Debug.Log("Non pénétrant");
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
        bool isCrit = false;
        float finalDamage = damage;
        if (team == Team.Player) 
        {
            isCrit = UnityEngine.Random.Range(1, 100 - this.pity) <= this.critChance;
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
            enemy.GetHealthManager().TakeDamage(finalDamage);
        }
        else if(target is PlayerController player)
        {
            player.GetHealthManager().TakeDamage(finalDamage);
        }

        ApplyLifeRip(lifeSteal);

        if(team == Team.Player)
        {
            GameManager.instance.IncreasePenetrativeShot();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// When an enemy is hit, compute if its penetrated, or if the shell do a ricochet
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="zone"></param>
    private void OnHittingEnemy(RaycastHit hit, HitZone zone)
    {
        EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
        if (enemy == null) return;

        float angle = GetAngle(hit, zone);

        ricochetAngle = ObtainRicochetAngle(zone);

        if (angle < ricochetAngle)
        {
            Debug.Log($"Hit : {zone.zoneName}, Angle : {angle}");

            if (CanPenetrate(zone.armorThickness, this.penetration, angle))
            {
                Debug.Log("Tir penetrant");
                PenetrateEnemy(enemy);
            }
            else
            {
                Debug.Log("Non penetrant");
                GameManager.instance.IncreaseNonPenetrativeShot();
                Destroy(gameObject);
             
            }
        }
        else
        {
            Debug.Log("Ricochet");
            Ricochet(hit);
        }
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
    /// Return true if we want to hit
    /// False if we want to go through
    /// </summary>
    /// <param name="gameObject">The gameObject the hit saw</param>
    /// <returns>True if the shell has to hit</returns>
    private bool CheckColliderTag(GameObject gameObject)
    {
        bool goThrough = false;

        switch (gameObject.tag) 
        {
            case "XP": 
                goThrough = false;
                break;

            case "Heal":
                goThrough = false;
                break;

            case "Ground":
                goThrough = false;
                break;

            case "Ennemy": 
                goThrough = true;
                break;

            default:
                goThrough = false;
            break;
        }
        return goThrough;
    }

    /// <summary>
    /// The shell penetrates the enemy, compute the critical damage and the liferip
    /// Destroy the shell after
    /// </summary>
    /// <param name="enemy">The enemy hit</param>
    private void PenetrateEnemy(EnemyController enemy)
    {
        if (UnityEngine.Random.Range(1, 100 - this.pity) <= this.critChance)
        {
            Debug.Log("critical hit");
            ApplyCriticalDamage(enemy);
        }
        else
        {
            ApplyDamage(enemy);
        }

        GameManager.instance.IncreasePenetrativeShot();

        // Apply the lifeSteal
        ApplyLifeRip(lifeSteal);

        // Destroy the shell
        Destroy(gameObject);
    }

    /// <summary>
    /// Apply Damage to the enemy, compute liferip and increase pity value
    /// </summary>
    /// <param name="enemy">The enemy hit</param>
    private void ApplyDamage(EnemyController enemy)
    {
        // Get the life steal proportion
        lifeSteal = GameManager.instance.GetPlayerController().GetHealthManager().GetLifeRip() * this.damage;

        // Apply the damage to the enemy
        enemy.GetHealthManager().TakeDamage(this.damage * this.critCoef);

        // Reset the pity when a crit happened
        IncreasePity();
    }

    /// <summary>
    /// Apply critical damage to the enemy, compute liferip and reset pity value to 0
    /// </summary>
    /// <param name="enemy">The enemy hit</param>
    private void ApplyCriticalDamage(EnemyController enemy)
    {
        // Get the life steal proportion
        lifeSteal = GameManager.instance.GetPlayerController().GetHealthManager().GetLifeRip() * this.damage * this.critCoef;

        // Apply the damage to the enemy
        enemy.GetHealthManager().TakeDamage(this.damage * this.critCoef);

        // enemy.GetComponent<EnemyHealthManager>().TakeDamage(this.damage * this.critCoef);

        // Reset the pity when a crit happened
        ResetPity();
    }

    /// <summary>
    /// Return the angle between the shell and the contact point
    /// </summary>
    /// <param name="hit"></param>
    /// <returns>The angle relative to normal of the collider</returns>
    private float GetAngle(RaycastHit hit, HitZone zone)
    {
        Debug.Log(Vector3.Angle(-direction, hit.normal));
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
