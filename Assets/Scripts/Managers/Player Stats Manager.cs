using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;
    [SerializeField] float healthAmount;
    [SerializeField] float fireRate;
    [SerializeField] private float regenHealthHPUpAmount = 2f;



    public bool updateHealth = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseHealth(int value = 1)
    {
        healthAmount += value;
        Actions.onHearthAmountUpgraded.Invoke(value);
    }
    private void Update()
    {
        if (updateHealth)
        {
            updateHealth = false;
            IncreaseHealth();
        }
    }
    public float GetHealthAmount()
    {
        return healthAmount;
    }

    public float GetHPUpRegenAmount()
    {
        return regenHealthHPUpAmount;
    }
}
