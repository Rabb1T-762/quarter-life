using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private float bulletVelocity = 30f;
        [SerializeField] private float bulletPrefabLifetime = 3f;

        [CanBeNull] private GameObject _currentBullet;

        public void FireWeapon()
        {
            _currentBullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            if (_currentBullet != null)
            {
                Rigidbody rb = _currentBullet.GetComponent<Rigidbody>();
                rb.AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
            }

            // Destroy the _currentBullet object if no impact
            StartCoroutine(DestroyBulletAfterTime(_currentBullet, bulletPrefabLifetime));
        }


        private IEnumerator DestroyBulletAfterTime(GameObject bulletObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (bulletObject)
            {
                bulletObject.GetComponent<Bullet>().DestroyBullet();
            }
        }
    }
}