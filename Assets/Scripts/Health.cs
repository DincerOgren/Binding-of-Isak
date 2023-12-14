using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float healthAmount=1;

    public Sprite hearthImage;
    public Sprite halfHearthImage;
    public Sprite emptyHearthImage;


    private bool isDead = false;




    public bool takeDamage = false;



    private float currentHealth;

    private void Start()
    {
        if (this.CompareTag("Player"))
        {
            healthAmount = PlayerStatsManager.instance.GetHealthAmount();
        }

        currentHealth = healthAmount;
    }


    private void Update()
    {
        if (takeDamage)
        {
            takeDamage = false;
            TakeDamage(.5f);
        }
    }

    public void TakeDamage(float damageAmount)      //instigator koyarsýn fonksiyona belki
    {

        if (!isDead)
        {

            currentHealth -= damageAmount;
            Actions.onTakeDamage(damageAmount);
            
        }
        if (currentHealth - damageAmount <= 0)
        {
            isDead = true;
            Die();
        }

    }

    private void Die()
    {
        Debug.LogError("Dead");
        //Destroy(gameObject); particle sfx etc.
    }

    public float GetCurrentHealthAmount()
    {
        return currentHealth;
    }
}
 
