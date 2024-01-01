using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class GridTest : MonoBehaviour
{
    public static GridTest instance;
    public GameObject obje;
    public Grid grid;
    public int xLen, yLen, cellSize;
    [SerializeField] Transform floorParent;
    [SerializeField] Transform roomVisual;

    public GameObject[] walkableFloorPrefabs;
    public GameObject[] obstacleFloorPrefabs;
//ss

    private void Awake()
    {
        instance = this;
    }
 
    void Start()
    {

        grid = new Grid(xLen, yLen, cellSize, roomVisual);
        AdjustGridValues();
        //StartCoroutine(SpawnRandomFloors());

    }

    public void AdjustGridValues()
    {
        foreach (Transform childTransform in floorParent.transform)
        {

            if (childTransform.TryGetComponent<Floor>(out var child))
            {
                if (child.IsWalkable())
                {
                    GridNode node = grid.GetGridNumber(child.transform.position);
                    node.value = 0;
                    //FOR CORNERS
                    if (node.number == new Vector2(xLen-1, 0))
                    {
                        //BOTTOM RIGHT
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.bottomRight;
                    }
                    else if (node.number == new Vector2(xLen - 1, yLen-1))
                    {
                        //TOP RIGHT
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.topRight;

                    }
                    else if (node.number == new Vector2(0, yLen - 1))
                    {
                        //TOP LEFT
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.topLeft;

                    }
                    else if (node.number == new Vector2(0, 0))
                    {
                        //BOTTOM LEFT
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.bottomLeft;

                    }
                    //FOR TOP BETWEEN
                    else if (node.number.y == yLen - 1)
                    {
                        //TOP LINE
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.topLine[UnityEngine.Random.Range(0, FloorManager.instance.cornerSprites.topLine.Length)];


                    }
                    else if (node.number.y == 0)
                    {
                        //BOTTOM LINE
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.bottomLine[UnityEngine.Random.Range(0, FloorManager.instance.cornerSprites.bottomLine.Length)];

                    }
                    else if (node.number.x == 0)
                    {
                        //LEFT LINE
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.leftLine[UnityEngine.Random.Range(0, FloorManager.instance.cornerSprites.leftLine.Length)];

                    }
                    else if (node.number.x == xLen - 1)
                    {
                        //RIGHT LINE
                        child.GetComponent<SpriteRenderer>().sprite = FloorManager.instance.cornerSprites.rightLine[UnityEngine.Random.Range(0, FloorManager.instance.cornerSprites.rightLine.Length)];

                    }
                    else 
                        child.SetRandomSpriteForFloor();

                    UpdateValueText(node);
                }
                else
                {
                    GridNode node = grid.GetGridNumber(child.transform.position);
                    node.value = 1;
                    UpdateValueText(node);
                    child.SetRandomSpriteForFloor();
                }
            }
        }


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
                Instantiate(obstacleFloorPrefabs[UnityEngine.Random.Range(0, obstacleFloorPrefabs.Length)], grid.GetCenterPointNode(item), Quaternion.Euler(0, 0, 0), floorParent);
            }
            else if (item.value == 0)
            {
                Instantiate(walkableFloorPrefabs[UnityEngine.Random.Range(0, walkableFloorPrefabs.Length)], grid.GetCenterPointNode(item), Quaternion.Euler(0, 0, 0), floorParent);
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
        if (Input.GetMouseButton(0))
        {
            var node = grid.GetGridNumber(UtilsClass.GetMouseWorldPosition());
            print("Grid [" + node.number.x + "," + node.number.y + "] , value = " + node.value);
        }

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
    public void UpdateValueText(GridNode node)
    {
        //Debug.Log("Updated node[" + node.number.x + ", " + node.number.y + "] , Fcost = " + node.fCost);
        grid.textArray[node.number.x, node.number.y].text = node.value.ToString();
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
