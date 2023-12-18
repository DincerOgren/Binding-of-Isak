using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class GridTest : MonoBehaviour
{
    public static GridTest instance;
    public GameObject obje;
    public Grid grid;
    public int xLen, yLen, cellSize;
    [SerializeField] Transform floorParent;

    public GameObject[] walkableFloorPrefabs;
    public GameObject[] obstacleFloorPrefabs;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        grid = new Grid(xLen, yLen, cellSize, transform);
        StartCoroutine(SpawnRandomFloors());

    }

    IEnumerator SpawnRandomFloors()
    {
        foreach (Transform item in floorParent)
        {
            Destroy(item.gameObject);
        }
        yield return null;
        foreach (var item in grid.GetGridArray())
        {
            if (item.value == 1)
            {
                Instantiate(obstacleFloorPrefabs[UnityEngine.Random.Range(0, obstacleFloorPrefabs.Length)], grid.GetCenterPoint(item), Quaternion.Euler(0, 0, 0), floorParent);
            }
            else if (item.value == 0)
            {
                Instantiate(walkableFloorPrefabs[UnityEngine.Random.Range(0, walkableFloorPrefabs.Length)], grid.GetCenterPoint(item), Quaternion.Euler(0, 0, 0), floorParent);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < xLen; i++)
        //{
        //    for (int j = 0; j < yLen; j++)
        //    {
        //       // grid.DrawCube(grid.gridStartPosition + (cellSize * i * Vector3.right) - (cellSize * j * Vector3.up), cellSize, grid.GetGridValue(i, j));
        //        //UtilsClass.CreateWorldText(grid.GetGridValue(i, j).ToString(), null, grid.GetWorldPosition(i, j), 10, Color.white, TextAnchor.MiddleCenter);
        //        Debug.LogWarning("Grid [" + i + "," + j + "] = " + grid.GetGridValue(i, j));
        //    }
        //}
        //DrawCube(Vector3.zero, new Vector3(grid.GetCellLength(), grid.GetCellHeight()));

        if (Input.GetKeyDown(KeyCode.L))
        {
            grid.RandomizeNodesValues();
            StartCoroutine(SpawnRandomFloors());
        }
        obje.transform.position = UtilsClass.GetMouseWorldPosition();
    }



    private void DrawCbe(Vector3 startPos, float size)
    {
        //Debug.DrawLine(startPos,startPos + Vector3.right * size.x);
        //Debug.DrawLine(startPos + (size.x *Vector3.right), startPos + (size.x * Vector3.right) + Vector3.down * size.y);
        //Debug.DrawLine(startPos+ new Vector3(size.x,-size.y,0f), startPos + new Vector3(size.x, -size.y, 0f)+Vector3.left* size.x);
        //Debug.DrawLine(startPos + (size.y * Vector3.down), startPos);

        Debug.DrawLine(startPos, startPos + size * Vector3.right);
        Debug.DrawLine(startPos, startPos + size * Vector3.down);

    }

    public void UpdateGText(GridNode node)
    {
        Debug.Log("Updated node[" + node.number.x + ", " + node.number.y + "] , Fcost = " + node.fCost);
        grid.textArray[node.number.x, node.number.y].text = node.value.ToString("F1");
    }

    public void UpdateText(int x, int y, string context)
    {
        grid.textArray[x, y].text = context;
    }

    public void WriteParentedNodes()
    {
        foreach (var item in grid.GetGridArray())
        {
            if (item.parent != null)
            {
                Debug.Log("Grid has parent [" + item.number.x + "," + item.number.y + "]");
            }
        }
    }


    public void TraverseReverse(GridNode node)
    {
        GridNode temp = node;
        while (temp != null)
        {
            Debug.Log("Grid[" + temp.number.x + "," + temp.number.y + "]");
            temp = temp.parent;
        }

    }
    private void OnDrawGizmos()
    {
        //for (int i = 0; i < 2; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        Gizmos.DrawWireCube(grid.startPos + (grid.GetCellLength() * j * Vector3.right) + (grid.GetCellHeight() * i * Vector3.up), new Vector3(grid.GetCellLength(), grid.GetCellHeight())); 
        //    }
        //}

    }



}
