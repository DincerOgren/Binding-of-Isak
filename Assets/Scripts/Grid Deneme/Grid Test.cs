using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public GameObject obje;
    Grid grid;
    public int xLen, yLen, cellSize;
    // Start is called before the first frame update
    void Start()
    {
        
        grid = new Grid(xLen, yLen, cellSize,transform);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < xLen; i++)
        {
            for (int j = 0; j < yLen; j++)
            {
               // grid.DrawCube(grid.gridStartPosition + (cellSize * i * Vector3.right) - (cellSize * j * Vector3.up), cellSize, grid.GetGridValue(i, j));
                //UtilsClass.CreateWorldText(grid.GetGridValue(i, j).ToString(), null, grid.GetWorldPosition(i, j), 10, Color.white, TextAnchor.MiddleCenter);
                Debug.LogWarning("Grid [" + i + "," + j + "] = " + grid.GetGridValue(i, j));
            }
        }
        //DrawCube(Vector3.zero, new Vector3(grid.GetCellLength(), grid.GetCellHeight()));

        if (Input.GetMouseButtonDown(0))
        {

            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 15);
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        if (Input.GetMouseButtonDown(1))
        {

            grid.SetValue(0,0,2);
            //grid.GetGridValue(grid.GetWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
        }
        obje.transform.position = UtilsClass.GetMouseWorldPosition();
    }

    

    private void DrawCbe(Vector3 startPos,float size)
    {
        //Debug.DrawLine(startPos,startPos + Vector3.right * size.x);
        //Debug.DrawLine(startPos + (size.x *Vector3.right), startPos + (size.x * Vector3.right) + Vector3.down * size.y);
        //Debug.DrawLine(startPos+ new Vector3(size.x,-size.y,0f), startPos + new Vector3(size.x, -size.y, 0f)+Vector3.left* size.x);
        //Debug.DrawLine(startPos + (size.y * Vector3.down), startPos);

        Debug.DrawLine(startPos, startPos + size * Vector3.right);
        Debug.DrawLine(startPos, startPos + size * Vector3.down);
        
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
