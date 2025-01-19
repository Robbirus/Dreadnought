using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeGraph.UnityCourse.MovingAround.Input
{
    public class StarshipController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference moveActionRef;

        [SerializeField]
        private float speed = 1f;

        private InputAction MoveAction => moveActionRef.action;

        void Start()
        {
            //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputAction.html
            MoveAction.Enable();
        }

        private void Update()
        {
            Vector2 movementInput = MoveAction.ReadValue<Vector2>();
            Move(movementInput);
        }

        private void Move(Vector2 input)
        {
            Vector3 translation = new Vector3
            {
                x = input.x * speed * Time.deltaTime,
                y = 0,
                z = input.y * speed * Time.deltaTime,
            };
            transform.localPosition += translation;
        }
    }

    //public class StarshipController : MonoBehaviour
    //{
    //    [SerializeField]
    //    private InputActionReference moveActionRef;

    //    [SerializeField]
    //    private float deceleration = 1f;
    //    [SerializeField]
    //    private float acceleration = 1f;
    //    [SerializeField]
    //    private float maxSpeed = 10f;

    //    private InputAction MoveAction => moveActionRef.action;
    //    private Vector2 movementDirection = Vector2.zero;
    //    private float currentSpeed = 0f;

    //    void Start()
    //    {
    //        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputAction.html
    //        MoveAction.Enable();
    //    }

    //    private void Update()
    //    {
    //        Vector2 movementInput = MoveAction.ReadValue<Vector2>();
    //        float newSpeed;
    //        if (movementInput.magnitude == 0)
    //        {
    //            newSpeed = currentSpeed - deceleration * Time.deltaTime;
    //        }
    //        else
    //        {
    //            newSpeed = currentSpeed + acceleration * Time.deltaTime;
    //            movementDirection = movementInput.normalized;
    //        }
    //        currentSpeed = Mathf.Clamp(newSpeed, 0, maxSpeed);
    //        Move(movementDirection * currentSpeed);
    //    }

    //    private void Move(Vector2 speed)
    //    {
    //        Vector3 translation = new Vector3
    //        {
    //            x = speed.x,
    //            y = 0,
    //            z = speed.y,
    //        };
    //        transform.localPosition += translation;
    //    }
    //}
}
