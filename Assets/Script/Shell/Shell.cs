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

    private void Start()
    {
        direction = transform.forward;

        direction.Normalize();

        critChance = GameManager.instance.GetGunManager().GetCritChance();
        critCoef = GameManager.instance.GetGunManager().GetCritCoef();
        pity = GameManager.instance.GetGunManager().GetPity();
    }

    /// <summary>
    /// Instantiate a shell depending of the type in the SO
    /// </summary>
    /// <param name="currentShell">The SO shell used this time</param>
    public void Setup(ShellSO currentShell)
    {
        int damageVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;
        int penetrationVariation = 1 + UnityEngine.Random.Range(-25, 25) / 100;

        penetration = currentShell.penetration * penetrationVariation;
        damage = currentShell.damage * damageVariation;
        velocity = currentShell.velocity;
        lifeTime = currentShell.lifeTime;

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
            if(CheckColliderTag(hit.collider.gameObject))
            {
                OnHittingEnemy(hit);
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

    /// <summary>
    /// When an enemy is hit, compute if its penetrated, or if the shell do a ricochet
    /// </summary>
    /// <param name="hit"></param>
    private void OnHittingEnemy(RaycastHit hit)
    {
        if (GetAngle(hit) < ricochetAngle)
        {
            GameObject contact = hit.collider.gameObject;
            Debug.Log(contact);
            Debug.Log(contact.transform.parent.gameObject);

            GameObject enemy = contact.transform.parent.gameObject;
            int enemyArmor = enemy.GetComponent<EnemyHealthManager>().GetArmor();

            if (CanPenetrate(enemyArmor, this.penetration, hit))
            {
                PenetrateEnemy(enemy);
            }
            else
            {
                GameManager.instance.IncreaseNonPenetrativeShot();
                Debug.Log("Non penetrant");
                Destroy(gameObject);
            }
        }
        else
        {
            Ricochet(hit);
        }
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
    private void PenetrateEnemy(GameObject enemy)
    {
        Debug.Log("Penetrating shot");
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
    private void ApplyDamage(GameObject enemy)
    {
        // Get the life steal proportion
        lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * this.damage;

        // Apply the damage to the enemy
        enemy.GetComponent<EnemyHealthManager>().TakeDamage(this.damage);

        // Reset the pity when a crit happened
        IncreasePity();
    }

    /// <summary>
    /// Apply critical damage to the enemy, compute liferip and reset pity value to 0
    /// </summary>
    /// <param name="enemy">The enemy hit</param>
    private void ApplyCriticalDamage(GameObject enemy)
    {
        // Get the life steal proportion
        lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * this.damage * this.critCoef;

        // Apply the damage to the enemy
        enemy.GetComponent<EnemyHealthManager>().TakeDamage(this.damage * this.critCoef);

        // Reset the pity when a crit happened
        ResetPity();
    }

    /// <summary>
    /// Return the angle between the shell and the contact point
    /// </summary>
    /// <param name="hit"></param>
    /// <returns>The angle relative to normal of the collider</returns>
    private float GetAngle(RaycastHit hit)
    {
        Debug.Log(Vector3.Angle(-direction, hit.normal));
        return Vector3.Angle(-direction, hit.normal);
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
    private bool CanPenetrate(int armor, int penetration, RaycastHit hit)
    {
        float relativeArmor = ObtainRelativeArmor(armor, hit);

        return penetration > relativeArmor;
    }

    /// <summary>
    /// Compute the relative armor using the angle and nominal armor
    /// </summary>
    /// <param name="armor">The enemy nominal armor</param>
    /// <param name="hit"></param>
    /// <returns>The relative armor of the enemy</returns>
    private float ObtainRelativeArmor(int armor, RaycastHit hit)
    {
        float angle = GetAngle(hit);
        
        return (armor)/Mathf.Cos(angle);
    }

    /// <summary>
    /// Check if LifeSteal Perk is obtained, if true, restore Health to the player equal of the life stolen
    /// if false, do nothing
    /// </summary>
    /// <param name="lifeSteal">The value of the life stole from the enemy</param>
    private void ApplyLifeRip(float lifeSteal)
    {
        if (GameManager.instance.GetPlayerHealthManager().IsLifeRipObtained())
        {
            GameManager.instance.GetPlayerHealthManager().RestoreHealth(lifeSteal);
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
