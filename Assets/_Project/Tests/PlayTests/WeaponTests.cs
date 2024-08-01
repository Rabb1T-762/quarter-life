using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using _Project.Tests.Mocks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using _Project.Tests.TestUtilities;
using NSubstitute;

public class WeaponTests
{
    private GameObject _weaponObject;
    private Weapon _weaponScript;
    private GameObject _bulletPrefab;
    private Transform _bulletSpawn;

    [SetUp]
    public void SetUp()
    {
        _weaponObject = new GameObject();
        _weaponScript = _weaponObject.AddComponent<Weapon>();

        _bulletPrefab = new GameObject();
        _bulletPrefab.AddComponent<Rigidbody>();

        _bulletSpawn = new GameObject().transform;
        TestHelper.SetPrivateField(_weaponScript, "bulletPrefab", _bulletPrefab);
        TestHelper.SetPrivateField(_weaponScript, "bulletSpawn", _bulletSpawn);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_weaponObject);
        Object.DestroyImmediate(_weaponScript);
        Object.DestroyImmediate(_bulletPrefab);
    }

    [UnityTest]
    public IEnumerator FireWeapon_InstantiatesBullet()
    {
        _weaponScript.FireWeapon();
        yield return null;

        Assert.IsNotNull(GameObject.FindObjectOfType<Rigidbody>(), "Bullet was not instantiated.");
    }

    [UnityTest]
    public IEnumerator FireWeapon_SetsBulletVelocity()
    {
        TestHelper.SetPrivateField(_weaponScript, "bulletVelocity", 50f);

        _weaponScript.FireWeapon();
        var testBullet = TestHelper.GetPrivateField<GameObject>(_weaponScript, "_currentBullet");
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(50f, testBullet.gameObject.GetComponent<Rigidbody>().velocity.magnitude, 0.1f,
            "Bullet velocity was not set correctly.");
    }

    [UnityTest]
    public IEnumerator FireWeapon_CallsDestroyBulletAfterLifetime()
    {
        var mockBulletPrefab = new GameObject();
        var mockBullet = mockBulletPrefab.AddComponent<BulletMock>();
        mockBulletPrefab.AddComponent<Rigidbody>();
        TestHelper.SetPrivateField(_weaponScript, "bulletPrefab", mockBulletPrefab);
        TestHelper.SetPrivateField(_weaponScript, "bulletPrefabLifetime", 0.1f);

        _weaponScript.FireWeapon();
        var testBullet = TestHelper.GetPrivateField<GameObject>(_weaponScript, "_currentBullet").GetComponent<BulletMock>();
        Assert.IsNotNull(testBullet);
        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(testBullet.DestroyBulletWasCalled, "Bullet was not destroyed after lifetime.");
    }

    [UnityTest]
    public IEnumerator FireWeapon_CallsDestroyBulletOnBullet()
    {
        var mockBulletPrefab = new GameObject();
        var mockBullet = mockBulletPrefab.AddComponent<BulletMock>();
        mockBulletPrefab.AddComponent<Rigidbody>();
        TestHelper.SetPrivateField(_weaponScript, "bulletPrefab", mockBulletPrefab);
        TestHelper.SetPrivateField(_weaponScript, "bulletPrefabLifetime", 0.1f);

        _weaponScript.FireWeapon();
        var testBullet = TestHelper.GetPrivateField<GameObject>(_weaponScript, "_currentBullet").GetComponent<BulletMock>();
        Assert.IsNotNull(testBullet);
        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(testBullet.DestroyBulletWasCalled, "DestroyBullet was not called on the currentBullet.");
    }
}