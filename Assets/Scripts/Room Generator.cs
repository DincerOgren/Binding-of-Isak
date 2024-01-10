using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

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
    public Transform roomParent;
    public Transform visual;
    public int cellSize = 1;
    public int desiredRoomAmount = 0;
    public int currentRoomAmount = 0;
    public bool spawnRooms = false;
    public int maxTry = 50;
    bool canSpawnRoom = false;
    public bool stepByStep = false;
    public List<Room> spawnedMultiLinkedRooms = new();
    public List<Room> rooms = new();
    public Room bossRoom;
    public Vector2Int centerPoint;
    public float totalWeight = 0;
    public float resetTimer = 0f;
    private void Start()
    {
        levelGrid = new Grid(levelGridX, levelGridY, cellSize, visual, false);

        foreach (var item in allRoomPrefabs)
        {
            totalWeight += item.roomWeightForRandomization;
        }
    }

    private void Update()
    {
        

        if (spawnRooms && resetTimer >= .5f)
        {
            spawnRooms = false;
            CreateRooms();
        }
        //test purposes
        else if (spawnRooms)
        {
            spawnRooms = false;

            CreateRooms();
        }
        resetTimer += Time.deltaTime;
    }

    private void CreateRooms()
    {
        desiredRoomAmount = CalculateRoomAmount();
        centerPoint = FindCenterNode(levelGrid.GetGridArray());
        StartCoroutine(SpawnRooms());
    }

    private Vector2Int FindCenterNode(GridNode[,] gridNodes)
    {
        return new Vector2Int(Mathf.FloorToInt(gridNodes.GetLength(0) / 2f), Mathf.FloorToInt(gridNodes.GetLength(1) / 2f));

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
        for (int i = 0; i < desiredRoomAmount * 2;)
        {
            if (stepByStep)
            {
                if (i >= spawnedMultiLinkedRooms.Count && currentRoomAmount < desiredRoomAmount)
                {

                    StartCoroutine(DestroyAllRooms());
                    resetTimer = 0f;
                    desiredRoomAmount = 0;
                    break;
                }
                else if (i >= spawnedMultiLinkedRooms.Count)
                {
                    Debug.Log("i passed the spawned rooms count, Process Completed");
                    break;
                }

                stepByStep = false;
                print(i + "= i    , spanwedRooms.COunt= " + spawnedMultiLinkedRooms.Count);
                if (spawnedMultiLinkedRooms[i].links.right && spawnedMultiLinkedRooms[i].attachedRooms.right == null)
                {
                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomRoom(leftRoomPrefabs);//LEFT
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
                        if (IsThereRoom(spawnedMultiLinkedRooms[i].number.x + 1, spawnedMultiLinkedRooms[i].number.y, out Room alreadyExsistedRoom))
                        {
                            Debug.LogError("ASDFSADFSADF" + alreadyExsistedRoom.name);
                            alreadyExsistedRoom.links.left = true;
                            Room tempRoom = null;

                            foreach (var item in leftRoomPrefabs)
                            {
                                if (item.links.right == alreadyExsistedRoom.links.right && item.links.left == alreadyExsistedRoom.links.left
                                    && item.links.top == alreadyExsistedRoom.links.top && item.links.bottom == alreadyExsistedRoom.links.bottom)
                                {
                                    Debug.LogError("NODE FIND");
                                    tempRoom = item; break;
                                }
                                Debug.LogWarning("Right= " + item.links.right + " Left= " + item.links.left + " Top= " + item.links.top + " Bot= " + item.links.bottom);
                                Debug.LogWarning("-----------Right= " + alreadyExsistedRoom.links.right + " Left= " + alreadyExsistedRoom.links.left + " Top= " + alreadyExsistedRoom.links.top + " Bot= " + alreadyExsistedRoom.links.bottom);
                            }
                            if (tempRoom == null)
                            {
                                Debug.LogError("NODE CANT FIND ON RIGHT");

                            }
                            if (tempRoom != null)
                            {

                                var spawnedRoom = Instantiate(tempRoom, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x + 1, spawnedMultiLinkedRooms[i].number.y), Quaternion.identity,roomParent);
                                spawnedMultiLinkedRooms[i].attachedRooms.right = spawnedRoom;
                                spawnedRoom.attachedRooms.left = spawnedMultiLinkedRooms[i];
                                spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x + 1;
                                spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y;

                                if (!rooms.Contains(spawnedRoom)) { rooms.Add(spawnedRoom); }
                                if (rooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = rooms.IndexOf(alreadyExsistedRoom);
                                    rooms.Remove(alreadyExsistedRoom);
                                    rooms.Insert(index, spawnedRoom);
                                }
                                if (spawnedMultiLinkedRooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = spawnedMultiLinkedRooms.IndexOf(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Remove(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Insert(index, spawnedRoom);
                                }

                                Destroy(alreadyExsistedRoom.gameObject);
                                print("Changed room on right link");

                            }
                        }
                        else
                        {

                            var spawnedRoom = Instantiate(room, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x + 1, spawnedMultiLinkedRooms[i].number.y), Quaternion.identity,roomParent);
                            spawnedMultiLinkedRooms[i].attachedRooms.right = spawnedRoom;
                            spawnedRoom.attachedRooms.left = spawnedMultiLinkedRooms[i];
                            spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x + 1;
                            spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y;
                            spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                            if (!rooms.Contains(spawnedRoom)) { rooms.Add(spawnedRoom); }

                            if (bossRoom.distanceToStartRoom < spawnedRoom.distanceToStartRoom)
                            {
                                bossRoom = spawnedRoom;
                            }
                            print("Spawned room on right link");
                            CheckLinkedRooms(spawnedRoom);
                            if (CheckRoomAmount(spawnedRoom) >= 1)
                            {
                                print("Added right link to spawnedRooms list");
                                spawnedMultiLinkedRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                print("Cant add right link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Room is null and breaking .. for the right");
                        break;
                    }
                }
                if (spawnedMultiLinkedRooms[i].links.left && spawnedMultiLinkedRooms[i].attachedRooms.left == null)
                {
                    int j = 0;

                    Room room;
                    while (true)
                    {
                        room = GetRandomRoom(rightRoomPrefabs);//RIGHT
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
                        if (IsThereRoom(spawnedMultiLinkedRooms[i].number.x - 1, spawnedMultiLinkedRooms[i].number.y, out Room alreadyExsistedRoom))
                        {
                            Debug.LogError("ASDFSADFSADF " + alreadyExsistedRoom.name);
                            alreadyExsistedRoom.links.right = true;

                            Room tempRoom = null;
                            foreach (var item in rightRoomPrefabs)
                            {
                                if (item.links.right == alreadyExsistedRoom.links.right && item.links.left == alreadyExsistedRoom.links.left
                                    && item.links.top == alreadyExsistedRoom.links.top && item.links.bottom == alreadyExsistedRoom.links.bottom)
                                {
                                    Debug.LogError("NODE FIND");
                                    tempRoom = item; break;
                                }
                                Debug.LogWarning("Right= " + item.links.right + " Left= " + item.links.left + " Top= " + item.links.top + " Bot= " + item.links.bottom);
                                Debug.LogWarning("-----------Right= " + alreadyExsistedRoom.links.right + " Left= " + alreadyExsistedRoom.links.left + " Top= " + alreadyExsistedRoom.links.top + " Bot= " + alreadyExsistedRoom.links.bottom);
                            }
                            if (tempRoom == null)
                            {
                                Debug.LogError("NODE CANT FIND ON LEFT");

                            }
                            if (tempRoom != null)
                            {


                                var spawnedRoom = Instantiate(tempRoom, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x - 1, spawnedMultiLinkedRooms[i].number.y), Quaternion.identity,roomParent);
                                spawnedMultiLinkedRooms[i].attachedRooms.left = spawnedRoom;
                                spawnedRoom.attachedRooms.right = spawnedMultiLinkedRooms[i];
                                spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x - 1;
                                spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y;
                                spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                                if (rooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = rooms.IndexOf(alreadyExsistedRoom);
                                    rooms.Remove(alreadyExsistedRoom);
                                    rooms.Insert(index, spawnedRoom);
                                }
                                if (spawnedMultiLinkedRooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = spawnedMultiLinkedRooms.IndexOf(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Remove(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Insert(index, spawnedRoom);
                                }
                                Destroy(alreadyExsistedRoom.gameObject);
                                print("Changed room on left link");

                            }
                        }
                        else
                        {
                            var spawnedRoom = Instantiate(room, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x - 1, spawnedMultiLinkedRooms[i].number.y), Quaternion.identity, roomParent);
                            spawnedMultiLinkedRooms[i].attachedRooms.left = spawnedRoom;
                            spawnedRoom.attachedRooms.right = spawnedMultiLinkedRooms[i];
                            spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x - 1;
                            spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y;
                            spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                            if (!rooms.Contains(spawnedRoom))
                            {
                                rooms.Add(spawnedRoom);
                            }

                            if (bossRoom.distanceToStartRoom < spawnedRoom.distanceToStartRoom)
                            {
                                bossRoom = spawnedRoom;
                            }
                            print("Spawned room on left link");

                            CheckLinkedRooms(spawnedRoom);
                            if (CheckRoomAmount(spawnedRoom) >= 1)
                            {
                                print("Added left link to spawnedRooms list");
                                spawnedMultiLinkedRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                print("Cant add left link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                            }
                        }
                    }
                    else
                    {
                        room = null;

                        Debug.LogWarning("Room is null and breaking .. for the left");
                        break;
                    }

                }
                if (spawnedMultiLinkedRooms[i].links.top && spawnedMultiLinkedRooms[i].attachedRooms.up == null)
                {
                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomRoom(downRoomPrefabs);//DOWN
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
                        if (IsThereRoom(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y + 1, out Room alreadyExsistedRoom))
                        {
                            Debug.LogError("ASDFSADFSADF" + alreadyExsistedRoom.name);
                            alreadyExsistedRoom.links.bottom = true;
                            Room tempRoom = null;
                            foreach (var item in downRoomPrefabs)
                            {
                                if (item.links.right == alreadyExsistedRoom.links.right && item.links.left == alreadyExsistedRoom.links.left
                                    && item.links.top == alreadyExsistedRoom.links.top && item.links.bottom == alreadyExsistedRoom.links.bottom)
                                {
                                    Debug.LogError("NODE FIND");
                                    tempRoom = item; break;
                                }
                                Debug.LogWarning("Right= " + item.links.right + " Left= " + item.links.left + " Top= " + item.links.top + " Bot= " + item.links.bottom);
                                Debug.LogWarning("-----------Right= " + alreadyExsistedRoom.links.right + " Left= " + alreadyExsistedRoom.links.left + " Top= " + alreadyExsistedRoom.links.top + " Bot= " + alreadyExsistedRoom.links.bottom);
                            }
                            if (tempRoom == null)
                            {
                                Debug.LogError("NODE CANT FIND ON TOP");
                            }
                            if (tempRoom != null)
                            {

                                var spawnedRoom = Instantiate(tempRoom, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y + 1), Quaternion.identity,roomParent);
                                spawnedMultiLinkedRooms[i].attachedRooms.up = spawnedRoom;
                                spawnedRoom.attachedRooms.down = spawnedMultiLinkedRooms[i];
                                spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x;
                                spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y + 1;
                                spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);

                                if (rooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = rooms.IndexOf(alreadyExsistedRoom);
                                    rooms.Remove(alreadyExsistedRoom);
                                    rooms.Insert(index, spawnedRoom);
                                }
                                if (spawnedMultiLinkedRooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = spawnedMultiLinkedRooms.IndexOf(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Remove(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Insert(index, spawnedRoom);
                                }
                                Destroy(alreadyExsistedRoom.gameObject);
                                print("Changed room on up link");

                            }
                        }
                        else
                        {
                            var spawnedRoom = Instantiate(room, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y + 1), Quaternion.identity, roomParent);
                            spawnedMultiLinkedRooms[i].attachedRooms.up = spawnedRoom;
                            spawnedRoom.attachedRooms.down = spawnedMultiLinkedRooms[i];
                            spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x;
                            spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y + 1;
                            spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                            if (!rooms.Contains(spawnedRoom))
                            {
                                rooms.Add(spawnedRoom);
                            }

                            if (bossRoom.distanceToStartRoom < spawnedRoom.distanceToStartRoom)
                            {
                                bossRoom = spawnedRoom;
                            }
                            print("Spawned room on up link");

                            CheckLinkedRooms(spawnedRoom);
                            if (CheckRoomAmount(spawnedRoom) >= 1)
                            {
                                print("Added up link to spawnedRooms list");
                                spawnedMultiLinkedRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                print("Cant add up link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Room is null and breaking .. for the up");
                        break;
                    }

                }
                if (spawnedMultiLinkedRooms[i].links.bottom && spawnedMultiLinkedRooms[i].attachedRooms.down == null)
                {

                    Room room;
                    int j = 0;
                    while (true)
                    {
                        room = GetRandomRoom(upRoomPrefabs);//UP ROOM
                        if (room.links.top && room.linkedRooms - 1 + currentRoomAmount <= desiredRoomAmount)
                        {
                            print("Found room on down link");

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
                        if (IsThereRoom(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y - 1, out Room alreadyExsistedRoom))
                        {
                            Debug.LogError("ASDFSADFSADF" + alreadyExsistedRoom.name);
                            alreadyExsistedRoom.links.top = true;
                            Room tempRoom = null;
                            foreach (var item in upRoomPrefabs)
                            {
                                if (item.links.right == alreadyExsistedRoom.links.right && item.links.left == alreadyExsistedRoom.links.left
                                    && item.links.top == alreadyExsistedRoom.links.top && item.links.bottom == alreadyExsistedRoom.links.bottom)
                                {
                                    Debug.LogError("NODE FIND");
                                    tempRoom = item; break;
                                }
                                Debug.LogWarning("Right= " + item.links.right + " Left= " + item.links.left + " Top= " + item.links.top + " Bot= " + item.links.bottom);
                                Debug.LogWarning("-----------Right= " + alreadyExsistedRoom.links.right + " Left= " + alreadyExsistedRoom.links.left + " Top= " + alreadyExsistedRoom.links.top + " Bot= " + alreadyExsistedRoom.links.bottom);
                            }
                            if (tempRoom == null)
                            {
                                Debug.LogError("NDOE CANT FINDON BOTTOM");
                            }
                            if (tempRoom != null)
                            {
                                var spawnedRoom = Instantiate(tempRoom, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y - 1), Quaternion.identity, roomParent);
                                spawnedMultiLinkedRooms[i].attachedRooms.down = spawnedRoom;
                                spawnedRoom.attachedRooms.up = spawnedMultiLinkedRooms[i];
                                spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x;
                                spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y - 1;
                                spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                                if (rooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = rooms.IndexOf(alreadyExsistedRoom);
                                    rooms.Remove(alreadyExsistedRoom);
                                    rooms.Insert(index, spawnedRoom);
                                }
                                if (spawnedMultiLinkedRooms.Contains(alreadyExsistedRoom))
                                {
                                    int index = spawnedMultiLinkedRooms.IndexOf(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Remove(alreadyExsistedRoom);
                                    spawnedMultiLinkedRooms.Insert(index, spawnedRoom);
                                }

                                Destroy(alreadyExsistedRoom.gameObject);

                                print("Changed room on down link");

                            }
                        }
                        else
                        {
                            var spawnedRoom = Instantiate(room, levelGrid.GetCenterPoint(spawnedMultiLinkedRooms[i].number.x, spawnedMultiLinkedRooms[i].number.y - 1), Quaternion.identity, roomParent);
                            spawnedMultiLinkedRooms[i].attachedRooms.down = spawnedRoom;
                            spawnedRoom.attachedRooms.up = spawnedMultiLinkedRooms[i];
                            spawnedRoom.number.x = spawnedMultiLinkedRooms[i].number.x;
                            spawnedRoom.number.y = spawnedMultiLinkedRooms[i].number.y - 1;
                            spawnedRoom.distanceToStartRoom = Vector2.Distance(centerPoint, spawnedRoom.number);
                            if (!rooms.Contains(spawnedRoom))
                            {
                                rooms.Add(spawnedRoom);
                            }

                            if (bossRoom.distanceToStartRoom < spawnedRoom.distanceToStartRoom)
                            {
                                bossRoom = spawnedRoom;
                            }
                            print("Spawned room on down link");

                            CheckLinkedRooms(spawnedRoom);
                            if (CheckRoomAmount(spawnedRoom) >= 1)
                            {
                                print("Added down link to spawnedRooms list");
                                spawnedMultiLinkedRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                print("Cant add down link to list links.linkedRooms = " + spawnedRoom.linkedRooms);
                            }
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

    private IEnumerator DestroyAllRooms()
    {
        List<Room> tempRooms = new(rooms);


        foreach (var item in tempRooms)
        {
            rooms.Remove(item);
            Destroy(item.gameObject);
        }
        rooms.Clear();

        List<Room> tempLinkedRooms = new(spawnedMultiLinkedRooms);


        foreach (var item in tempLinkedRooms)
        {
            spawnedMultiLinkedRooms.Remove(item);
            Destroy(item.gameObject);
        }
        spawnedMultiLinkedRooms.Clear();

        yield return null;


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

    private bool IsThereRoom(int x, int y, out Room room)
    {
        foreach (var item in rooms)
        {
            if (item.number == new Vector2Int(x, y))
            {
                room = item;
                return true;
            }
        }
        room = null;
        return false;
    }

    private void CheckLinkedRooms(Room room)
    {
        int tempLinkRoom = room.linkedRooms;
        if (room.attachedRooms.right != null)
        {
            tempLinkRoom--;
        }
        else if (room.links.right)
        {
            if (IsThereRoom(room.number.x + 1, room.number.y, out _))
            {
                tempLinkRoom--;
            }

        }
        if (room.attachedRooms.left != null)
        {
            tempLinkRoom--;
        }
        else if (room.links.left)
        {
            if (IsThereRoom(room.number.x - 1, room.number.y, out _))
            {
                tempLinkRoom--;
            }
        }
        if (room.attachedRooms.up != null)
        {
            tempLinkRoom--;
        }
        else if (room.links.top)
        {
            if (IsThereRoom(room.number.x, room.number.y + 1, out _))
            {
                tempLinkRoom--;
            }

        }
        if (room.attachedRooms.down != null)
        {
            tempLinkRoom--;
        }
        else if (room.links.bottom)
        {
            if (IsThereRoom(room.number.x, room.number.y - 1, out _))
            {
                tempLinkRoom--;
            }

        }
        currentRoomAmount += tempLinkRoom;
    }

    private void SpawnStarterRoom()
    {
        var room = GetRandomStarterRoom();
        var spawnedRoom = Instantiate(room, levelGrid.GetCenterPoint(centerPoint.x, centerPoint.y), Quaternion.identity, roomParent);
        currentRoomAmount += spawnedRoom.linkedRooms + 1;
        spawnedRoom.number = centerPoint;
        spawnedMultiLinkedRooms.Add(spawnedRoom);
        rooms.Add(spawnedRoom);
        bossRoom = spawnedRoom;
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
    private Room GetRandomRoom(List<Room> items)
    {
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < items.Count; i++)
        {
            cumulativeWeight += items[i].roomWeightForRandomization;
            if (randomValue <= cumulativeWeight)
            {
                return items[i];
            }
        }

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
