using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeGraph.UnityCourse.Projectiles.CharacterController
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference fireAction;
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private float force = 1f;

        private void Start()
        {
            fireAction.action.Enable();
            fireAction.action.performed += FirePerformed;
        }

        private void FirePerformed(InputAction.CallbackContext obj)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            var body = projectile.GetComponent<Rigidbody>();
            body.AddForce(transform.forward * force);
        }
    }
}
