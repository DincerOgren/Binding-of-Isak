using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Grid;

public class AStarPathfindingDeneme : MonoBehaviour
{
    public static AStarPathfindingDeneme instance;


    public List<GridNode> openList = new List<GridNode>();
    public List<GridNode> closeList = new List<GridNode>();




    public GridNode currentNode;


    public GridNode startNode;
    public GridNode targetNode;




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
        print(Mathf.Sqrt(1));
        if (Input.GetMouseButtonDown(0))
        {

            if (isFirst)
            {

                SetStartNode(GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));
                isFirst = false;
            }
            var list = GridTest.instance.grid.FindNeighbours(GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));

            CalculateValues(list,startNode,targetNode);


            foreach (GridNode node in list)
            {
                Debug.LogError("Node[" + node.number.x + "," + node.number.y + "]" + " gcost= " + node.gCost + ", hcost = " + node.hCost + " , fcost = " + node.fCost);
            }

            // grid.SetValue(UtilsClass.GetMouseWorldPosition(), 15);
            //Debug.LogError(grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        if (Input.GetMouseButtonDown(1))
        {

            SetTargetNode();
            
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }



        //while (true)
        //{
        //    currentNode = GetLowestInOpenList();

        //    if (currentNode == targetNode)
        //    {
        //        break;
        //    }

        //    GridTest.instance.grid.FindNeighbours(currentNode);
        //}

    }

    private void CalculateValues(List<GridNode> gridNodes, GridNode startNode, GridNode targetNode)
    {
        foreach (var item in gridNodes)
        {
            item.gCost = CalculateGCost(item, startNode);
            item.hCost = CalculateHCost(item, targetNode);
            item.fCost = CalculateFCost(item);
        }
    }

    private float CalculateGCost(GridNode item, GridNode start)
    {
        return Mathf.Sqrt((((item.number.x - start.number.x) * (item.number.x - start.number.x)) *  GridTest.instance.grid.GetCellSize()) + (((item.number.y - start.number.y) * (item.number.y - start.number.y)) * GridTest.instance.grid.GetCellSize()));
    }

    private float CalculateHCost(GridNode item, GridNode target)
    {
        return Mathf.Sqrt((((item.number.x - target.number.x) * (item.number.x - target.number.x))  * GridTest.instance.grid.GetCellSize()) + (((item.number.y - target.number.y) * (item.number.y - target.number.y)) * GridTest.instance.grid.GetCellSize()));
    }

    private float CalculateFCost(GridNode item)
    {
        return item.hCost + item.gCost;
    }

    private void SetTargetNode()
    {
        targetNode = GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition());
    }

    private GridNode GetLowestInOpenList()
    {
        GridNode temp = null;
        for (int i = 0; i < openList.Count; i++)
        {
            temp = openList[i];
            if (temp.fCost < openList[i].fCost)
            {
                temp = openList[i];
            }
        }
        if (temp != null)
        {
            openList.Remove(temp);
            closeList.Add(temp);
        }
        return temp;
    }

    public void SetStartNode(GridNode a)
    {
        startNode = a;
        openList.Add(a);
    }
    private float CalculateDistance(Vector2 first, Vector2 second)
    {
        float x = Mathf.Abs(first.x - second.x);
        float y = Mathf.Abs(first.y - second.y);

        return Mathf.Sqrt(x * x + y * y);
    }

}
