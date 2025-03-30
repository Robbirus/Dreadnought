using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
    [Header("Center of Mass")]
    [SerializeField] private Vector3 centerOfMass;
    [SerializeField] bool awake;
    [SerializeField] private Rigidbody body;

    private void Update()
    {
        body.centerOfMass = centerOfMass;
        body.WakeUp();
        awake = !body.IsSleeping();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
    }
}
