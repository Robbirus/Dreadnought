using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Script instance")]
    [SerializeField] private PlayerHealthManager healthManager;
    [SerializeField] private GunManager gunManager;
    [SerializeField] private ExperienceManager xpManager;
    [SerializeField] private PlayerMovement movement;


    #region Getter / Setter
    public PlayerHealthManager GetHealthManager()
    {
        return this.healthManager;
    }

    public ExperienceManager GetXpManager()
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
