using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{


    [SerializeField] bool isWalkable = false;

    public void SetRandomSpriteForFloor()
    {
        if (isWalkable)
        {
            GetComponent<SpriteRenderer>().sprite = FloorManager.instance.GetRandomWalkableFloorSprite();
        }
        if (!isWalkable)
        {
            GetComponent<SpriteRenderer>().sprite = FloorManager.instance.GetRandomObstacleFloorSprite();
        }
    }
    public bool IsWalkable() => isWalkable;
}
