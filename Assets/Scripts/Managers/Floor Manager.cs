using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager instance;

    public Sprite[] obstacleSprites;
    public Sprite commonFloorSprite;
    public Sprite[] rareFloorSprites;

    public int floorSelectorSeed;
    public int obstacleSelectorSeed ;

    public bool useSeed = false;


    public WeightedRandomSelector<WeigthedSprite> obstacleSelector = new WeightedRandomSelector<WeigthedSprite>(true);
    
    public WeightedRandomSelector<WeigthedSprite> floorSelector = new(false);
    void Start()
    {
        instance = this;



        WeigthedSprite commonSprite = new() { sprite = commonFloorSprite, weight = 10 };
        floorSelector.AddItem(commonSprite, commonSprite.weight);
        for (int i = 0; i < rareFloorSprites.Length; i++)
        {
            WeigthedSprite rareSprite = new() { sprite = rareFloorSprites[i], weight = 1 };
            floorSelector.AddItem(rareSprite, rareSprite.weight);
        }

        for (int i = 0; i < obstacleSprites.Length; i++)
        {
            WeigthedSprite rareSprite = new() { sprite = obstacleSprites[i], weight = 1 };
            obstacleSelector.AddItem(rareSprite, rareSprite.weight);
        }
        SetSeeds();

        DontDestroyOnLoad(gameObject);
    }

    private void RandomizeSeeds()
    {
        floorSelectorSeed= UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        obstacleSelectorSeed= UnityEngine.Random.Range(int.MinValue,int.MaxValue);
    }
    private void SetSeeds()
    {
        obstacleSelector.SetSeed(obstacleSelectorSeed);
        floorSelector.SetSeed(floorSelectorSeed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            RandomizeSeeds();
            SetSeeds();
            GridTest.instance.AdjustGridValues();
        }

        Debug.Log("Obstacle Seed= " + obstacleSelector.seed);
        Debug.Log("Floor Seed= " + floorSelector.seed);
    }

    public Sprite GetRandomWalkableFloorSprite()
    {
        return floorSelector.GetRandomItem().sprite;
    }


    public Sprite GetRandomObstacleFloorSprite()
    {
        return obstacleSelector.GetRandomItem().sprite;
    }
}

[System.Serializable]
public class WeigthedSprite
{
    public Sprite sprite { get; set; }
    public float weight { get; set; }
}
