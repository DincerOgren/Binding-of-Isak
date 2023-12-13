using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float healthAmount=1;

    public Sprite hearthImage;
    public Sprite halfHearthImage;
    public Sprite emptyHearthImage;









    private float currentHealth;

    private void Awake()
    {
        if (this.CompareTag("Player"))
        {
            healthAmount = PlayerStatsManager.instance.GetHealthAmount();
        }

        currentHealth = healthAmount;
    }
}
