using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header("Shell Attribut")]
    [SerializeField]
    private float shellSpeed = 200;
    [SerializeField]
    private float lifeTime = 10f;

    private float damage;
    private int critChance;
    private float critCoef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = GameManager.instance.GetGunManager().GetDamage();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = gameObject.transform.forward * shellSpeed;
    }

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
}
