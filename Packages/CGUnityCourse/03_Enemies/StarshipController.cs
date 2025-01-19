using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeGraph.UnityCourse.Enemies.CharacterController
{

    [RequireComponent(typeof(UnityEngine.CharacterController))]
    public class StarshipController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference moveActionRef;

        [SerializeField]
        private float forceFactor = 1f;

        private InputAction MoveAction => moveActionRef.action;
        private UnityEngine.CharacterController controller;

        void Start()
        {
            controller = GetComponent<UnityEngine.CharacterController>();
            MoveAction.Enable();
        }

        private void Update()
        {
            Vector2 movementInput = MoveAction.ReadValue<Vector2>();
            Move(movementInput);
        }

        private void Move(Vector2 movementInput)
        {
            Vector3 force = new Vector3
            {
                x = movementInput.x * forceFactor * Time.deltaTime,
                y = 0,
                z = movementInput.y * forceFactor * Time.deltaTime,
            };
            controller.Move(force);
        }
    }
}
