using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{

    [SerializeField]
    private InputActionReference moveActionReference;
    [SerializeField]
    private InputActionReference boostActionReference;
    [SerializeField]
    private float speed = 4f;

    private Vector3 A;
    private Vector3 B;

    public int interpolationFramesCount = 60; // Number of frames to completely interpolate between the 2 positions
    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        A = new Vector3(0, 0, 0);
        B = new Vector3(1, 0, 1);
        startTime = Time.time;
        moveActionReference.action.Enable();
        boostActionReference.action.Enable();
        speed = 4f;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 frameMovement = moveActionReference.action.ReadValue<Vector2>();
        Vector3 frameMovement3D = new Vector3(frameMovement.x, 0, frameMovement.y);
        Vector3 newPosition;
        if (boostActionReference.action.IsPressed() == true) {

            newPosition = gameObject.transform.position + frameMovement3D * 5 * speed * Time.deltaTime;
        } else {

            newPosition = gameObject.transform.position + frameMovement3D * speed * Time.deltaTime;
        }

        Vector3 direction = newPosition - gameObject.transform.position;
        gameObject.transform.position = newPosition;
        if (direction.magnitude != 0)
        {
            Quaternion tankOrientation = Quaternion.LookRotation(direction, Vector3.up);
            gameObject.transform.rotation = tankOrientation;
        }


    }
}
