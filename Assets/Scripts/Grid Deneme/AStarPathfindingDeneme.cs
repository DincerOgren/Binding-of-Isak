using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Grid;
using static UnityEditor.Progress;

public class AStarPathfindingDeneme : MonoBehaviour
{
    public static AStarPathfindingDeneme instance;


    public List<GridNode> openList = new List<GridNode>();
    public List<GridNode> closeList = new List<GridNode>();




    public GridNode currentNode;


    public GridNode startNode;
    public GridNode targetNode;


    public bool stepByStep = false;
    public bool traverse = false;
    //DENEME 
    public bool shouldCalculate = false;
    public bool isFirst = true;
    public Vector2 first, second;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartTRaverseMethod());




            // grid.SetValue(UtilsClass.GetMouseWorldPosition(), 15);
            //Debug.LogError(grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        if (Input.GetMouseButtonDown(1))
        {

            SetTargetNode();

            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GridTest.instance.WriteParentedNodes();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GridTest.instance.TraverseReverse(targetNode);

        }
    }

   

    IEnumerator StartTRaverseMethod()
    {
        if (isFirst)
        {

            SetStartNode();
            isFirst = false;
            var list = GridTest.instance.grid.FindNeighbours(GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));

            CalculateValues(list, GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()), targetNode);
        }
        


       




        if (traverse)
        {

            while (true)
            {

                if (!stepByStep)
                {

                    currentNode = GetLowestInOpenList();

                    openList.Remove(currentNode);
                    closeList.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        Debug.LogError("Path Finded");
                        break;
                    }

                    var liste = GridTest.instance.grid.FindNeighbours(currentNode);
                    foreach (var neighbour in liste)
                    {
                        if (closeList.Contains(neighbour))
                        {
                            continue;
                        }

                        if (NewPath(neighbour) < neighbour.gCost || !openList.Contains(neighbour))
                        {
                            neighbour.gCost = NewPath(neighbour);
                            neighbour.hCost = CalculateHCost(neighbour, targetNode);
                            neighbour.fCost = CalculateFCost(neighbour);
                            neighbour.parent = currentNode;


                            if (!openList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                            }
                        }
                        GridTest.instance.UpdateGText(neighbour);

                    }
                }
                stepByStep = true;
                yield return null;
            }
        }
    }

    private float NewPath(GridNode neighbour)
    {
        return Mathf.Sqrt((((neighbour.number.x - startNode.number.x) * (neighbour.number.x - startNode.number.x)) * GridTest.instance.grid.GetCellSize()) + (((neighbour.number.y - startNode.number.y) * (neighbour.number.y - startNode.number.y)) * GridTest.instance.grid.GetCellSize()));
    }


    private void CalculateValues(List<GridNode> gridNodes, GridNode startNode, GridNode targetNode)
    {
        foreach (var item in gridNodes)
        {
            item.gCost = CalculateGCost(item, startNode);
            item.hCost = CalculateHCost(item, targetNode);
            item.fCost = CalculateFCost(item);
            GridTest.instance.UpdateGText(item);
        }
    }
    private float CalculateGCost(GridNode item, GridNode start)
    {
        return Mathf.Sqrt((((item.number.x - start.number.x) * (item.number.x - start.number.x)) * GridTest.instance.grid.GetCellSize()) + (((item.number.y - start.number.y) * (item.number.y - start.number.y)) * GridTest.instance.grid.GetCellSize()));
    }
    private float CalculateHCost(GridNode item, GridNode target)
    {
        return Mathf.Sqrt((((item.number.x - target.number.x) * (item.number.x - target.number.x)) * GridTest.instance.grid.GetCellSize()) + (((item.number.y - target.number.y) * (item.number.y - target.number.y)) * GridTest.instance.grid.GetCellSize()));
    }
    private float CalculateFCost(GridNode item)
    {
        return item.hCost + item.gCost;
    }
   

    private GridNode GetLowestInOpenList()
    {
        GridNode temp = null;
        temp = openList[0];
        foreach (var item in openList)
        {
            if (item.fCost < temp.fCost)
            {
                temp = item;
            }
        }
        
        
           
        
        return temp;
    }

    public void SetStartNode()
    {
        startNode = GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition());
    
        openList.Add(startNode);
    }
    private void SetTargetNode()
    {
        targetNode = GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition());
    }
    private float CalculateDistance(Vector2 first, Vector2 second)
    {
        float x = Mathf.Abs(first.x - second.x);
        float y = Mathf.Abs(first.y - second.y);

        return Mathf.Sqrt(x * x + y * y);
    }

}
