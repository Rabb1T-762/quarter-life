using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private float bulletVelocity = 30f;
        [SerializeField] private float bulletPrefabLifetime = 3f;

        public void FireWeapon()
        {
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            // Shoot the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
            // Destroy the bullet after a certain amount of time
            StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));
        }

        private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }
    }
}