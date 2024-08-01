using _Project.Scripts;
using UnityEngine;

namespace _Project.Tests.Mocks
{
    public class BulletMock : Bullet
    {
        public bool DestroyBulletWasCalled { get; private set; }

        public override void DestroyBullet()
        {
            Debug.Log("I'm a mock bullet");
            DestroyBulletWasCalled = true;
            base.DestroyBullet();
        }
        
    }
}