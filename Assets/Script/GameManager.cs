using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GameObject UI Instance")]
    [SerializeField]
    private GameObject needle;

    [Header("Script Instance")]
    [SerializeField]
    private TankController tankController;
    [SerializeField]
    private PlayerHealthManager healthManager;
    [SerializeField]
    private ExperienceManager experienceManager;

    private float tankSpeed;

    private float startPosition = 220f;
    private float endPosition = -41f ;
    private float desiredPosition;


    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(healthManager.GetHealth() <= 0f)
        {
            SceneManager.LoadScene("Game Over Screen");
        }

        tankSpeed = tankController.GetCurrentSpeed();
        UpdateNeedle();
    }

    private void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = tankSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));   
    }
}
