using UnityEngine;

namespace _Project.Scripts
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Target"))
            {
                print("Hit " + collision.gameObject.name + "!");
                Destroy(gameObject);
            }
        
            if (collision.gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}
