using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Shell : MonoBehaviour
{
    [Header("Shell Attribut")]
    [SerializeField] private float shellSpeed = 200;
    [SerializeField] private float lifeTime = 10f;

    private Vector3 direction;

    private float damage;
    private int critChance;
    private float critCoef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = transform.forward;
        damage = GameManager.instance.GetGunManager().GetDamage();
        critChance = GameManager.instance.GetGunManager().GetCritChance();
        critCoef = GameManager.instance.GetGunManager().GetCritCoef();
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
