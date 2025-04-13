using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Script instance")]
    [SerializeField] private PlayerHealthManager healthManager;


    #region Getter / Setter
    public PlayerHealthManager GetHealthManager()
    {
        return this.healthManager;
    }

    #endregion

}
