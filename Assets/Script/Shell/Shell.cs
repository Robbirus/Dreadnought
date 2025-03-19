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
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += direction * shellSpeed * Time.fixedDeltaTime;
    }

    #region Getter / Setter
    public float GetCritChance()
    {
        return critChance;
    }
    public float GetCritCoef()
    {
        return critCoef;
    }
    public float GetDamage()
    {
        return damage;
    }
    #endregion
}
