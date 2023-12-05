using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShadow : MonoBehaviour
{
    public Transform bulletVisual;
    public float bulletDropTime = 2f;
    public float desiredDropDuration = 1f;
    public AnimationCurve dropSpeedCurve;
    private float timer = 0f;
    private bool isTimerRunning = false;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            if (bulletDropTime < timer)
            {
                if (bulletVisual.localPosition.y <= 0.4f)
                {
                    bulletVisual.localPosition = Vector3.zero;
                    rb.velocity = Vector2.zero;
                    //Destroy?
                }
                else
                    bulletVisual.localPosition = Vector3.Lerp(bulletVisual.localPosition, new Vector3(0, 0, 0), Time.deltaTime * desiredDropDuration);
            }
        }
    }


    public void SetSpeedForBullet(Vector2 speedVar)
    {
        rb.velocity = speedVar;
        isTimerRunning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("COllsion var = " + collision.name);
        //if (collision.CompareTag("PlayerBulletVisual"))
        //{
        //    // Particle effect

        //    Destroy(collision.gameObject);
        //}
    }
}
