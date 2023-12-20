using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeightedRandomSelector<T>
{
    private List<T> items = new();
    private List<float> weights = new();
    private float totalWeight = 0;
    public int seed = 123;
    public bool useSeed = false;

    public WeightedRandomSelector(bool useSeed)
    {
        this.useSeed = useSeed;

    }

    public void SetSeed(int newSeed)
    {
        if (useSeed)
        {
            seed=newSeed;
            UnityEngine.Random.InitState(newSeed);
        }
        else
        {
            seed = Random.Range(0,100000);
            UnityEngine.Random.InitState(seed);
        }
    }

    public void AddItem(T item, float weight)
    {
        items.Add(item);
        weights.Add(weight);
        totalWeight += weight;
    }

    public T GetRandomItem()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("No items added to the WeightedRandomSelector.");
        }


        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < items.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return items[i];
            }
        }

        return items[^1];
    }
}
