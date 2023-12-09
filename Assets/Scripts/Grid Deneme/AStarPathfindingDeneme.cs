using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Grid;

public class AStarPathfindingDeneme : MonoBehaviour
{
    public static AStarPathfindingDeneme instance;


    public List<GridNode> openList=new List<GridNode>();
    public List<GridNode> closeList=new List<GridNode>();




    public GridNode currentNode;

    public GridNode targetNode;




    //DENEME 
    public bool shouldCalculate = false;
    public bool isFirst = false;
    public Vector2 first, second;

    void Start()
    {
        instance= this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            
                //SetStartNode(GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));

            var list = GridTest.instance.grid.FindNeighbours(GridTest.instance.grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));


            foreach (GridNode node in list)
            {
                Debug.LogError("Node[" + node.number.x + "," + node.number.y + "]");
            }

            // grid.SetValue(UtilsClass.GetMouseWorldPosition(), 15);
            //Debug.LogError(grid.GetGridNumber(UtilsClass.GetMouseWorldPosition()));
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        if (Input.GetMouseButtonDown(1))
        {

            Debug.LogWarning(CalculateDistance(first, second));

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
        if (temp!=null)
        {
            openList.Remove(temp);
            closeList.Add(temp);
        }
        return temp;
    }

    public void SetStartNode(GridNode a)
    {
        openList.Add(a);
    }
    private float CalculateDistance(Vector2 first, Vector2 second)
    {
        float x = Mathf.Abs(first.x - second.x);
        float y = Mathf.Abs(first.y - second.y);

        return Mathf.Sqrt(x * x + y * y);
    }

}
