using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Bullets/Bullet Prototype")]
public class BulletPrototypeSO : ScriptableObject
{
    public GameObject bulletPrefab;
    public GameObject evilBulletPrefab;

    public float bulletSpeed;
    private GameObject bulletToFire;


    public int PickANumber()
    {
        return Random.Range(1, 6);
    }

    public GameObject ChooseBulletToShot()
    {
        if (PickANumber() == 1)
        {
            bulletToFire = evilBulletPrefab;
        }
        else
            bulletToFire = bulletPrefab;

        return bulletToFire;
    }
}
