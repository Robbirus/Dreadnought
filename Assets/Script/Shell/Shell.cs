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

        Destroy(gameObject, lifeTime);
    }

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

    /*
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;
        Vector3 incoming = direction;

        float angle = Vector3.Angle(-incoming, normal);

        Debug.Log(angle);

        if (angle < ricochetAngle)
        {
            direction = Vector3.Reflect(incoming, normal);
            transform.forward = direction; // Pour l'orientation visuelle
        }
        else
        {
            // Impact direct : détruire ou exploser
            Destroy(gameObject);
        }
    }
    */

    private void Update()
    {
        //transform.position += direction * velocity * Time.fixedDeltaTime; 
        
        Vector3 nextPos = transform.position + direction * velocity * Time.deltaTime;
        Vector3 move = nextPos - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, move.magnitude))
        {
            if(CheckColliderTag(hit.collider.gameObject))
            {
                if(GetAngle(hit) < ricochetAngle)
                {
                    GameObject enemy = hit.collider.gameObject;
                    Debug.Log(enemy);
                    int enemyArmor = enemy.GetComponent<EnemyHealthManager>().GetArmor();

                    if (CanPenetrate(enemyArmor, this.penetration, hit))
                    {
                        PenetrateEnemy(enemy);
                    }
                    else
                    {
                        Debug.Log("Non penetrant");
                    }
                }
                else
                {
                    Ricochet(hit);
                }
            }
            else
            {
                transform.position = nextPos;
            }
        }
        else
        {
            transform.position = nextPos;
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

    private void PenetrateEnemy(GameObject enemy)
    {
        if (UnityEngine.Random.Range(1, 100 - this.pity) <= this.critChance)
        {
            ApplyCriticalDamage(enemy);
        }
        else
        {
            ApplyDamage(enemy);
        }

        // Apply the lifeSteal
        ApplyLifeRip(lifeSteal);

        // Destroy the shell
        Destroy(gameObject);
    }

    private void ApplyDamage(GameObject enemy)
    {
        // Get the life steal proportion
        lifeSteal = GameManager.instance.GetPlayerHealthManager().GetLifeRip() * this.damage;

        // Apply the damage to the enemy
        enemy.GetComponent<EnemyHealthManager>().TakeDamage(this.damage);

        // Reset the pity when a crit happened
        IncreasePity();
    }

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
            direction = Vector3.Reflect(direction, hit.normal);
            transform.position = hit.point;
            this.penetration *= (75/100);
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="armor"></param>
    /// <param name="penetration"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    private bool CanPenetrate(int armor, int penetration, RaycastHit hit)
    {
        float relativeArmor = ObtainRelativeArmor(armor, hit);

        return penetration > relativeArmor;
    }

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
