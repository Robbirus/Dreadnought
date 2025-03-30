using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour
{
    [Header("Shell Attribut")]
    [Tooltip("Base Shell velocity")]
    [SerializeField] private float shellSpeed = 200;
    [Tooltip("Base Shell's lifetime in seconds")]
    [SerializeField] private float lifeTime = 10f;

    private Vector3 direction;

    private float damage;
    private int critChance;
    private float critCoef;
    private int pity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = transform.forward;
        damage = GameManager.instance.GetGunManager().GetDamage();
        critChance = GameManager.instance.GetGunManager().GetCritChance();
        critCoef = GameManager.instance.GetGunManager().GetCritCoef();
        pity = GameManager.instance.GetGunManager().GetPity();

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += direction * shellSpeed * Time.fixedDeltaTime;
    }

    #region Getter / Setter
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
