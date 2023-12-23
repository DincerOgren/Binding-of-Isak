using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] int level = 1;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void UpdateLevel(int amount=1)
    {
        level += amount;
    }

    public int GetLevel()
    {
        return level;
    }
}
