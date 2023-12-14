using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [SerializeField] float healthAmount;
    [SerializeField] float currentHealthAmount;
    [SerializeField] GameObject hearthUIPrefab;
    [SerializeField] Sprite fullHearthUISprite;
    [SerializeField] Sprite halfHearthUISprite;
    [SerializeField] Sprite emptyHearthUISprite;
    [SerializeField] GameObject hearthContainer;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        healthAmount = PlayerStatsManager.instance.GetHealthAmount();
        currentHealthAmount = healthAmount;
        SpawnHearthUI();

        DontDestroyOnLoad(gameObject);
    }

    
    private void OnEnable()
    {
        Actions.onTakeDamage += UpdateCurrentHealth;
        Actions.onHearthAmountUpgraded += UpdateHeartsAmount;
    }
    private void OnDisable()
    {
        Actions.onTakeDamage-= UpdateCurrentHealth;
        Actions.onHearthAmountUpgraded -= UpdateHeartsAmount;

    }
    private void UpdateHeartsAmount(float value)
    {
        healthAmount += value;

        SpawnHearthUI();      

    }

    public void UpdateCurrentHealth(float value)
    {
        Debug.Log("Value= " + value);
        currentHealthAmount -= value;
        AdjustHearthImages();
    }


    void AdjustHearthImages()
    {
        float tempCurr = currentHealthAmount;
        int i = 0;
        while (true)
        {
            if (i == healthAmount)
            {
                break;
            }

            if (tempCurr - 1 >= 0)
            {
                print("fullhearth girildi value= " + tempCurr + " i value= " + i);
                hearthContainer.transform.GetChild(i).GetComponent<Image>().sprite = fullHearthUISprite;
                tempCurr -= 1;
                i++;
                continue;
            }
            else if (tempCurr - .5 == 0)
            {
                print("elif girildi value= " + tempCurr + " i value= " + i);

                hearthContainer.transform.GetChild(i).GetComponent<Image>().sprite = halfHearthUISprite;
                tempCurr -= .5f;
                i++;
                continue;
            }
            else
            {
                print("ese girildi value= " + tempCurr+ " i value= "+i);

                hearthContainer.transform.GetChild(i).GetComponent<Image>().sprite = emptyHearthUISprite;
                i++;
            }
        }

    }
    private void SpawnHearthUI()
    {
        foreach (Transform childTransform in hearthContainer.transform)
        {
            Destroy(childTransform.gameObject);
        }
        for (int i = 0; i < healthAmount; i++)
        {
            Instantiate(hearthUIPrefab, hearthContainer.transform);
        }
    }

}
