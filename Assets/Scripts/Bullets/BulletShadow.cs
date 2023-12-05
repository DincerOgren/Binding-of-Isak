using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShadow : MonoBehaviour
{
    [Header("Bullet Visual")]
    public Transform bulletVisual;
    public float bulletDropTime = 2f;
    public float desiredDropDuration = 1f;
    public float visualStartHeight = 3f;
    public AnimationCurve bulletDropCurve;
    [Header("Values for Bullet High")]
    public float maxHitHeightForBullet = 3f;
    private float timer = 0f;
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        bulletVisual.localPosition = new Vector3(0, visualStartHeight, 0);
    }
    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            if (bulletDropTime < timer)
            {
                if (bulletVisual.localPosition.y <= 0.2f)
                {
                    bulletVisual.localPosition = Vector3.zero;
                    rb.velocity = Vector2.zero;
                    //Destroy?
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                    float percentageComplete = elapsedTime / desiredDropDuration;
                    bulletVisual.localPosition = Vector3.Lerp(bulletVisual.localPosition, new Vector3(0, 0, 0), bulletDropCurve.Evaluate(percentageComplete));
                }
            }
        }
    }


    public void SetSpeedForBullet(Vector2 speedVar)
    {
        rb.velocity = speedVar;
        isTimerRunning = true;
    }

    
    
        //print("COllsion var = " + collision.name);
        //if (collision.CompareTag("PlayerBulletVisual"))
        //{
        //    // Particle effect

        //    Destroy(collision.gameObject);
        //}

        
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (bulletVisual.localPosition.y <= maxHitHeightForBullet)
            {
                print("Hit the enemy and height is acceptable ENTER");
                Destroy(collision.gameObject);
            }
            else
                print("Hit but height is above max limit ENTER");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (bulletVisual.localPosition.y <= maxHitHeightForBullet)
            {
                print("Hit the enemy and height is acceptable STAY");
                Destroy(collision.gameObject);
            }
            else
                print("Hit but height is above max limit STAY");
        }
    }
}
