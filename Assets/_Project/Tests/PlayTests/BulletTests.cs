using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using _Project.Tests.TestUtilities;


public class BulletTests
{
    private GameObject _bulletObject;
    private Bullet _bulletScript;

    [SetUp]
    public void SetUp()
    {
        _bulletObject = new GameObject();
        _bulletScript = _bulletObject.AddComponent<Bullet>();
        _bulletObject.AddComponent<Rigidbody>();
        _bulletObject.AddComponent<CapsuleCollider>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_bulletObject);
    }

    [UnityTest]
    public IEnumerator BulletIsDestroyedAfterDestroyBulletIsCalled()
    {
        // Arrange
        var bullet = Object.Instantiate(new GameObject());
        var bulletScript = bullet.AddComponent<Bullet>();
        bullet.AddComponent<Rigidbody>(); 

        // Act
        bulletScript.DestroyBullet();

        // Wait for the next frame to allow Unity to process the destruction
        yield return null;

        // Assert
        Assert.IsTrue(bullet == null);
    }

    [UnityTest]
    public IEnumerator BulletDestroysItselfUponCollision()
    {
        // Arrange 
        
        var colliderObject = new GameObject("ColliderObject");
        var boxCollider = colliderObject.AddComponent<BoxCollider>();
        // Position it in front of the bullet
        colliderObject.transform.position = new Vector3(0, 0, 5);

        
        // Act 
        // Position bullet to collide with the colliderObject
        _bulletObject.transform.position = new Vector3(0, 0, 0);
        _bulletObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 100); 
        
        // Wait for collision to occur
        yield return new WaitForSeconds(0.1f); 

        bool isBulletDestroyed = _bulletObject == null;
        
        // Assert
        Assert.IsTrue(isBulletDestroyed);
    }
}