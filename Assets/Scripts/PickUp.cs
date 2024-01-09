using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] PickupType pickUpType;
    [SerializeField] int pickupAmount = 1;
//A
    private void Update()
    {
        if (pickUpType == PickupType.Bomb)
        {
            GameManager.instance.AddBomb(pickupAmount);
        }
        else if (pickUpType == PickupType.Gold)
        {
            GameManager.instance.AddGold(pickupAmount);

        }
        else if (pickUpType == PickupType.PowerUp)
        {
            //we can add later
        }
    }


}

public enum PickupType
{
    Bomb,
    Gold,
    PowerUp
}