using System;
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
                DestroyBullet();
            }
        
            if (collision.gameObject)
            {
                DestroyBullet();
            }
        }
        
        public virtual void DestroyBullet()
        {
            Debug.Log("Destroyed Bullet");
            Destroy(gameObject);
        }
    }
}
