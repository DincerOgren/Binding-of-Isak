using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int goldCount = 0;
    [SerializeField] int bombCount = 0;


    public bool bomb = false;
    public bool gold = false;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
         if (bomb) { bomb = false; AddBomb(); }
         if (gold) { gold = false; AddGold(); }
    }


    public int GetBombAmount() => bombCount;
    public int GetGoldAmount() => goldCount;

    public void AddGold(int amount = 1) => goldCount += amount;
    public void AddBomb(int amount = 1) => bombCount += amount;

}
