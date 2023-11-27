using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterPrototype : MonoBehaviour
{
    public BulletPrototypeSO prototypeSO;

    public GameObject bulletPrefab;

    public float shootCooldown = 1f;
    public float divideAmount = 2f;

    private float timer = Mathf.Infinity;

    public Vector2 fireInput;



    Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb= GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        HandleInput();
        timer += Time.deltaTime;   
    }

    private void HandleInput()
    {
        float xInput = Input.GetAxisRaw("FireHorizontal");
        float yInput = Input.GetAxisRaw("FireVertical");

        fireInput= new Vector2 (xInput,yInput);

        if (fireInput.y > 0)
        {
            if (timer > shootCooldown)
            {
                timer = 0;
                CanFireBullet(Vector2.up);
            }
        }
        if (fireInput.y < 0)
        {
            if (timer > shootCooldown)
            {
                timer = 0;
                CanFireBullet(Vector2.down);
            }
        }
        if (fireInput.x < 0)
        {
            if (timer > shootCooldown)
            {
                timer = 0;
                CanFireBullet(Vector2.left);
            }
        }
        if (fireInput.x > 0)
        {
            if (timer > shootCooldown)
            {
                timer = 0;
                CanFireBullet(Vector2.right);
            }
        }
    }

    private void CanFireBullet(Vector2 dir)
    {
        bulletPrefab = prototypeSO.ChooseBulletToShot();
        var spawnedPrefab = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        spawnedPrefab.GetComponent<Rigidbody2D>().velocity = prototypeSO.bulletSpeed *( dir + GetMovementDir()/divideAmount).normalized;

        
    }

    private Vector2 GetMovementDir()
    {
        var a = playerRb.velocity;
        if (a.magnitude > 3f)
        {
            return a;
        }
        else
            return Vector2.zero;
    }
}
