using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterPrototype : MonoBehaviour
{
    public BulletPrototypeSO prototypeSO;

    public GameObject bulletPrefab;
    public Transform[] bulletExitPoints;
    public float sameDirectionBulletSpeedMultiplier = 1.5f;
    public float shootCooldown = 1f;
    public float divideAmount = 2f;

    private float timer = Mathf.Infinity;

    public Vector2 fireInput;

    int exitPointNum = 0;

    Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
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

        fireInput = new Vector2(xInput, yInput);

        if (fireInput.y > 0)
        {
            if (timer > shootCooldown)
            {
                RotateHead(0);
                timer = 0;
                CanFireBullet(Vector2.up);
            }
        }
        if (fireInput.y < 0)
        {
            if (timer > shootCooldown)
            {
                RotateHead(180);
                timer = 0;
                CanFireBullet(Vector2.down);
            }
        }
        if (fireInput.x < 0)
        {
            if (timer > shootCooldown)
            {
                RotateHead(90);
                timer = 0;
                CanFireBullet(Vector2.left);
            }
        }
        if (fireInput.x > 0)
        {
            if (timer > shootCooldown)
            {
                RotateHead(270);
                timer = 0;
                CanFireBullet(Vector2.right);
            }
        }
    }

    private void RotateHead(float angle)
    {
        transform.eulerAngles = new(0, 0, angle);
    }

    private void CanFireBullet(Vector2 dir)
    {
        print(Vector2.Dot(dir.normalized, GetMovementDir().normalized));
        bulletPrefab = prototypeSO.ChooseBulletToShot();
        if (exitPointNum > 0)
        {
            var spawnedPrefab = Instantiate(bulletPrefab, bulletExitPoints[exitPointNum].position, Quaternion.identity);
            SetShootDirection(dir, spawnedPrefab);
            exitPointNum = 0;
        }
        else if (exitPointNum == 0)
        {
            var spawnedPrefab = Instantiate(bulletPrefab, bulletExitPoints[exitPointNum].position, Quaternion.identity);
            SetShootDirection(dir, spawnedPrefab);
            exitPointNum++;
        }
    }

    private void SetShootDirection(Vector2 dir, GameObject spawnedPrefab)
    {
        //IF PLAYER SHOOTS THE OPPOSITE DIRECTON OF HIS MOVEMENT
        if (Vector2.Dot(dir.normalized, GetMovementDir().normalized) < 0)
        {
            print("-1 ateslendi");
            spawnedPrefab.GetComponent<BulletShadow>().SetSpeedForBullet(prototypeSO.bulletSpeed * dir.normalized);
        }
        // IF PLAYER SHOOTS THE SAME WAY OF HIS MOVEMENT DIRECTION
        else if (Vector2.Dot(dir.normalized, GetMovementDir().normalized) > 0)
        {
            print("1 ateslendi");
            spawnedPrefab.GetComponent<BulletShadow>().SetSpeedForBullet(prototypeSO.bulletSpeed * sameDirectionBulletSpeedMultiplier * dir.normalized);
        }
        else
        {
            print("else ateslendi");
            spawnedPrefab.GetComponent<BulletShadow>().SetSpeedForBullet(prototypeSO.bulletSpeed * (dir + (GetMovementDir() / divideAmount)).normalized);
        }
    }

    private Vector2 GetMovementDir()
    {
        var a = playerRb.velocity;
        if (a.magnitude > 3f)
        {
            return a;
        }
        else
            return a/2;
    }
}
