using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float healthAmount=1;

    [Header("For Player")]
    public Sprite hearthImage;
    public Sprite halfHearthImage;
    public Sprite emptyHearthImage;

    private bool isDead = false;


//FOR TEST PURPOSES

    public bool takeDamage = false;



    private float currentHealth;


    private void OnEnable()
    {
        Actions.onHearthAmountUpgraded += RegenHealthWhenHPUp;
    }

    private void OnDisable()
    {
        Actions.onHearthAmountUpgraded-= RegenHealthWhenHPUp;

    }
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

    public void TakeDamage(float damageAmount)      //instigator koyars�n fonksiyona belki
    {

        if (!isDead)
        {

            currentHealth = Mathf.Max(currentHealth - damageAmount,0);
            Actions.onTakeDamage(damageAmount);
            
        }

        if (currentHealth <= 0)
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

    private void RegenHealthWhenHPUp(float v)
    {
        healthAmount += v;
        RegenHealth(PlayerStatsManager.instance.GetHPUpRegenAmount());
    }
    private void RegenHealth(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, healthAmount);
    }
    public float GetCurrentHealthAmount()
    {
        return currentHealth;
    }
}
 
