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
        StartCoroutine(SpawnHearthUI());
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
        currentHealthAmount = Mathf.Min(currentHealthAmount + PlayerStatsManager.instance.GetHPUpRegenAmount(), healthAmount);

        StartCoroutine(SpawnHearthUI());
    }

    public void UpdateCurrentHealth(float value)
    {
        Debug.Log("Value= " + value);
        currentHealthAmount -= value;
        AdjustHearthImages();
    }

    private IEnumerator SpawnHearthUI()
    {
        // YOU CAN UPDATE THIS METHOD FOR VISUALLY PREFECT
        // PLAYER CAN SEE THE CHANGES IN THE ONE FRAME
        foreach (Transform childTransform in hearthContainer.transform)
        {
            Destroy(childTransform.gameObject);
        }

        yield return null;

        for (int i = 0; i < healthAmount; i++)
        {
            Instantiate(hearthUIPrefab, hearthContainer.transform);
        }

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
                print("if girildi value= " + tempCurr + " i value= " + i);

                if (hearthContainer.transform.GetChild(i).TryGetComponent<Image>(out Image imgComp))
                {
                    imgComp.sprite = fullHearthUISprite;
                    print("if degistirdi resim + i =" + imgComp.name);
                }
                else
                    print("image component not found ++ if");
                tempCurr -= 1;
                i++;
                continue;
            }
            else if (tempCurr - .5 >= 0)
            {
                print("elif girildi value= " + tempCurr + " i value= " + i);

                if(hearthContainer.transform.GetChild(i).TryGetComponent<Image>(out Image imgComp))
                {
                    imgComp.sprite = halfHearthUISprite;
                    print("elif degistirdi resim + i =" + imgComp.name);
                }
                else
                    print("image component not found ++ elif");
                tempCurr -= .5f;
                i++;
                continue;
            }
            else
            {
                print("ese girildi value= " + tempCurr+ " i value= "+i);

                if (hearthContainer.transform.GetChild(i).TryGetComponent<Image>(out Image imgComp))
                {
                    imgComp.sprite = emptyHearthUISprite;
                    print("else degistirdi resim + i =" + imgComp.name);
                }
                else
                    print("image component not found ++ else");
                i++;
            }
        }

    }
    

}
