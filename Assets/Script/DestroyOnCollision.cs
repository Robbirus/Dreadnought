using UnityEngine;

namespace CodeGraph.UnityCourse.Enemies.CharacterController 
{ 
    public class DestroyOnCollision : MonoBehaviour
    {
        private void OnTriggerEnter()
        {
                Debug.Log($"Trigger enter started on gameObject {gameObject.name}");
                Destroy(gameObject);
        }
    }   
}