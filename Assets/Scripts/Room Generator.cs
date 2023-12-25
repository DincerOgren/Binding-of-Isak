using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public List<Room> allRoomPrefabs;
    public List<Room> leftRoomPrefabs;
    public List<Room> rightRoomPrefabs;
    public List<Room> upRoomPrefabs;
    public List<Room> downRoomPrefabs;
    Grid levelGrid;
    //They should be odd
    public int levelGridX, levelGridY;

    public Transform visual;
    public int cellSize = 1;
    public int desiredRoomAmount = 0;
    public int currentRoomAmount = 0;
    public bool spawnRooms = false;
    public int maxTry = 50;
    bool canSpawnRoom = false;
    public bool stepByStep = false;
    public List<Room> spawnedRooms = new();

    public Vector2Int centerPoint;
    private void Start()
    {
        levelGrid = new Grid(levelGridX, levelGridY, cellSize, visual,false);
    }

    private void Update()
    {
        if (spawnRooms)
        {
            spawnRooms = false;

            CreateRooms();
        }
    }

    private void CreateRooms()
    {
        desiredRoomAmount = CalculateRoomAmount();
        centerPoint = FindCenterNode(levelGrid.GetGridArray());
        StartCoroutine(SpawnRooms());
    }

    private Vector2Int FindCenterNode(GridNode[,] gridNodes)
    {
        return new Vector2Int(Mathf.CeilToInt(gridNodes.GetLength(0) / 2f), Mathf.CeilToInt(gridNodes.GetLength(1) / 2f));

        //// E�er grid boyutlar� tek say�daysa, orta nokta
        //if (gridWidth % 2 == 0)
        //{
        //    centerX -= 1; // �nceki h�creyi se�mek i�in bir azaltma
        //}

        //if (gridHeight % 2 == 0)
        //{
        //    centerY -= 1; // �nceki h�creyi se�mek i�in bir azaltma
        //}


    }

    private IEnumerator SpawnRooms()
    {
        SpawnStarterRoom();
        for (int i = 0; i < desiredRoomAmount*2;)
        {
            if (stepByStep)
            {
                if (i >= spawnedRooms.Count)
                {
                    Debug.Log("i passed the spawned rooms count, Process Completed");
                    break;
                }
                stepByStep = false;
                print(i + "= i    , spanwedRooms.COunt= " + spawnedRooms.Count);
                if (spawnedRooms[i].links.right && spawnedRooms[i].attachedRooms.right == null)
                {
                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomLeftRoom();
                        if (room.links.left && room.linkedRooms - 1 + currentRoomAmount <= desiredRoomAmount)
                        {
                            print("Found room on right link");
                            break;
                        }
                        if (j > maxTry)
                        {
                            room = null;
                            print("Over max try in down link i=" + i + " ,j= " + j);

                            break;
                        }
                        j++;
                    }
                    if (room != null)
                    {
                        var spawnedRoom = Instantiate(room, levelGrid.GetWorldPosition(spawnedRooms[i].number.x + 1, spawnedRooms[i].number.y), Quaternion.identity);
                        spawnedRooms[i].attachedRooms.right = spawnedRoom;
                        spawnedRoom.attachedRooms.left = spawnedRooms[i];
                        spawnedRoom.number.x = spawnedRooms[i].number.x + 1;
                        spawnedRoom.number.y = spawnedRooms[i].number.y;
                        print("Spawned room on right link");
                        CheckLinkedRooms(spawnedRoom);
                        if (CheckRoomAmount(spawnedRoom) >= 1)
                        {
                            print("Added right link to spawnedRooms list");
                            spawnedRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            print("Cant add right link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Room is null and breaking .. for the right");
                        break;
                    }
                }
                if (spawnedRooms[i].links.left && spawnedRooms[i].attachedRooms.left == null)
                {
                    int j = 0;

                    Room room;
                    while (true)
                    {
                        room = GetRandomRightRoom();
                        if (room.links.right && room.linkedRooms - 1 + currentRoomAmount <= desiredRoomAmount)
                        {
                            print("Found room on left link");

                            break;
                        }
                        if (j > maxTry)
                        {
                            room = null;

                            print("Over max try in down link i=" + i + " ,j= " + j);


                            break;
                        }
                        j++;
                    }
                    if (room != null)
                    {

                        var spawnedRoom = Instantiate(room, levelGrid.GetWorldPosition(spawnedRooms[i].number.x - 1, spawnedRooms[i].number.y), Quaternion.identity);
                        spawnedRooms[i].attachedRooms.left = spawnedRoom;
                        spawnedRoom.attachedRooms.right = spawnedRooms[i];
                        spawnedRoom.number.x = spawnedRooms[i].number.x - 1;
                        spawnedRoom.number.y = spawnedRooms[i].number.y;
                        print("Spawned room on left link");

                        CheckLinkedRooms(spawnedRoom);
                        if (CheckRoomAmount(spawnedRoom) >= 1)
                        {
                            print("Added left link to spawnedRooms list");
                            spawnedRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            print("Cant add left link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                        }
                    }
                    else
                    {
                        room = null;

                        Debug.LogWarning("Room is null and breaking .. for the left");
                        break;
                    }

                }
                if (spawnedRooms[i].links.top && spawnedRooms[i].attachedRooms.up == null)
                {
                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomDownRoom();
                        if (room.links.bottom && room.linkedRooms - 1 + currentRoomAmount <= desiredRoomAmount)
                        {
                            print("Found room on up link");

                            break;
                        }
                        if (j > maxTry)
                        {
                            print("Over max try in down link i=" + i + " ,j= " + j);


                            break;
                        }
                        j++;
                    }
                    if (room != null)
                    {

                        var spawnedRoom = Instantiate(room, levelGrid.GetWorldPosition(spawnedRooms[i].number.x, spawnedRooms[i].number.y + 1), Quaternion.identity);
                        spawnedRooms[i].attachedRooms.up = spawnedRoom;
                        spawnedRoom.attachedRooms.down = spawnedRooms[i];
                        spawnedRoom.number.x = spawnedRooms[i].number.x;
                        spawnedRoom.number.y = spawnedRooms[i].number.y + 1;
                        print("Spawned room on up link");

                        CheckLinkedRooms(spawnedRoom);
                        if (CheckRoomAmount(spawnedRoom) >= 1)
                        {
                            print("Added up link to spawnedRooms list");
                            spawnedRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            print("Cant add up link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Room is null and breaking .. for the up");
                        break;
                    }

                }
                if (spawnedRooms[i].links.bottom && spawnedRooms[i].attachedRooms.down == null)
                {

                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomUpRoom();
                        if (room.links.top && room.linkedRooms - 1 + currentRoomAmount <= desiredRoomAmount)
                        {
                            print("Found room on down link");

                            break;
                        }
                        if (j > maxTry)
                        {
                            room = null;

                            print("Over max try in down link i=" + i+" ,j= "+j);

                            break;
                        }
                        j++;
                    }
                    if (room != null)
                    {

                        var spawnedRoom = Instantiate(room, levelGrid.GetWorldPosition(spawnedRooms[i].number.x, spawnedRooms[i].number.y - 1), Quaternion.identity);
                        spawnedRooms[i].attachedRooms.down = spawnedRoom;
                        spawnedRoom.attachedRooms.up = spawnedRooms[i];
                        spawnedRoom.number.x = spawnedRooms[i].number.x;
                        spawnedRoom.number.y = spawnedRooms[i].number.y - 1;
                        print("Spawned room on down link");

                        CheckLinkedRooms(spawnedRoom);
                        if (CheckRoomAmount(spawnedRoom) >= 1)
                        {
                            print("Added down link to spawnedRooms list");
                            spawnedRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            print("Cant add down link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Room is null and breaking .. for the down");
                        break;
                    }
                }

               

                i++;
            }
            else
                yield return null;
        }
       

    }

    private int CheckRoomAmount(Room room)
    {
        int tempLinkRoom = room.linkedRooms;
        if (room.attachedRooms.right != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.left != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.up != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.down != null)
        {
            tempLinkRoom--;
        }

        return tempLinkRoom;
    }

    private void CheckLinkedRooms(Room room)
    {
        int tempLinkRoom = room.linkedRooms;
        if (room.attachedRooms.right != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.left != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.up != null)
        {
            tempLinkRoom--;
        }
        if (room.attachedRooms.down != null)
        {
            tempLinkRoom--;
        }

        currentRoomAmount += tempLinkRoom;
    }

    private void SpawnStarterRoom()
    {
        var room = GetRandomStarterRoom();
        var spawnedRoom = Instantiate(room, levelGrid.GetWorldPosition(centerPoint.x, centerPoint.y), Quaternion.identity);
        currentRoomAmount += spawnedRoom.linkedRooms + 1;
        spawnedRoom.number = centerPoint;
        spawnedRooms.Add(spawnedRoom);
    }

    private Room GetRandomStarterRoom()
    {
        Room room;

        while (true)
        {
            room = allRoomPrefabs[UnityEngine.Random.Range(0, allRoomPrefabs.Count)];
            if (room.linkedRooms > 1)
            {
                break;
            }
        }

        return room;
    }
    private Room GetRandomRoom()
    {
        return allRoomPrefabs[UnityEngine.Random.Range(0, allRoomPrefabs.Count)];
    }
    private Room GetRandomLeftRoom()
    {
        return leftRoomPrefabs[UnityEngine.Random.Range(0, leftRoomPrefabs.Count)];
    }
    private Room GetRandomRightRoom()
    {
        return rightRoomPrefabs[UnityEngine.Random.Range(0, rightRoomPrefabs.Count)];
    }
    private Room GetRandomUpRoom()
    {
        return upRoomPrefabs[UnityEngine.Random.Range(0, upRoomPrefabs.Count)];
    }
    private Room GetRandomDownRoom()
    {
        return downRoomPrefabs[UnityEngine.Random.Range(0, downRoomPrefabs.Count)];
    }
    private int CalculateRoomAmount()
    {
        return Mathf.FloorToInt(UnityEngine.Random.Range(0, 3) + 5 + (LevelManager.instance.GetLevel() * 2.6f));
    }
}