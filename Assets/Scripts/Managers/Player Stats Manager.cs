using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;
    [SerializeField] float healthAmount;
    [SerializeField] float fireRate;
    
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
    }

    public float GetHealthAmount()
    {
        return healthAmount;
    }
}
