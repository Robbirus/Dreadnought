using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject needle;
    [SerializeField]
    private TankController tankController;

    private float tankSpeed;

    private float startPosition = 220f;
    private float endPosition = -41f ;
    private float desiredPosition;


    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
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
