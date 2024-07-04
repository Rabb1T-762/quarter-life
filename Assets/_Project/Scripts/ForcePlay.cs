using UnityEngine;

namespace _Project.Scripts
{
    public class ForcePlay : MonoBehaviour
    {
        [SerializeField] private new Transform transform;
        [SerializeField] private float forceMagnitude = 10f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 forceDirection;
        private InputManager _inputManager;

        void Start()
        {
            _inputManager = GetComponent<InputManager>();
            rb = transform.GetComponent<Rigidbody>();
        }

        void Update() 
        {
            bool jumpInput = _inputManager.GetJumpInput();
            if(jumpInput)
            {
                // Debug.Log("About to apply a force!");
                ApplyForce();
            }
        }

        private void ApplyForce()
        {
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }

    }
}