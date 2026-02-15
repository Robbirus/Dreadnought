using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Script instance")]
    [SerializeField] private PlayerHealthManager healthManager;
    [SerializeField] private GunManager gunManager;
    [SerializeField] private PlayerExperienceManager xpManager;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerStatsSO playerData;

    private void Awake()
    {
        GameManager.instance.RegisterPlayer(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.UnregisterPlayer(this);
    }

    private void Start()
    {
        healthManager.SetMaxHealth(playerData.maxHealth);
        healthManager.SetArmor(playerData.armor);
        movement.SetMaxSpeed(playerData.maxSpeed);
        movement.SetRotationSpeed(playerData.rotationSpeed);
    }


    #region Getter / Setter
    public PlayerHealthManager GetHealthManager()
    {
        return this.healthManager;
    }

    public PlayerExperienceManager GetXpManager()
    {
        return this.xpManager;
    }

    public GunManager GetGunManager()
    {
        return this.gunManager;
    }
    public PlayerMovement GetMovement()
    {
        return this.movement;
    }
    #endregion

}
