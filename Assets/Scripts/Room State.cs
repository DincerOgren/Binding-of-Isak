using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomState : MonoBehaviour
{

    public Transform player;
    public RoomStates roomState;

    public bool playerInRoom = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        roomState = RoomStates.empty;
    }

    private void Update()
    {
        if (playerInRoom)
        {
            roomState = RoomStates.lockedInRoom;
        }
        if (!playerInRoom)
        {
            roomState = RoomStates.roomClear;
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRoom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRoom = false;
        }
    }

    public RoomStates GetRoomStates()
    {
        return roomState;
    }
}

public enum RoomStates
{
    lockedInRoom,
    roomClear,
    empty

}
