using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [SerializeField] float healthAmount;
    [SerializeField] GameObject hearthUIPrefab;
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

        foreach (Transform childTransform in hearthContainer.transform)
        {
            Destroy(childTransform.gameObject);
        }
        for (int i = 0; i < healthAmount; i++)
        {
            Instantiate(hearthUIPrefab, hearthContainer.transform);

        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
