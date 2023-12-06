using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    Grid grid;
    public int xLen, yLen, cellLen, cellHeight;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        grid = new Grid(xLen, yLen, cellLen, cellHeight);
        for (int i = 0; i < yLen; i++)
        {
            for (int j = 0; j < xLen; j++)
            {
                DrawCube(grid.startPos + (grid.GetCellLength() * j * Vector3.right) + (grid.GetCellHeight() * i * Vector3.up), new Vector3(grid.GetCellLength(), grid.GetCellHeight()));
            }
        }
        // DrawCube(Vector3.zero, new Vector3(grid.GetCellLength(), grid.GetCellHeight()));    
    }

    private void DrawCube(Vector3 startPos,Vector3 size)
    {
        Debug.DrawLine(startPos,startPos + Vector3.right * size.x);
        Debug.DrawLine(startPos + (size.x *Vector3.right), startPos + (size.x * Vector3.right) + Vector3.down * size.y);
        Debug.DrawLine(startPos+ new Vector3(size.x,-size.y,0f), startPos + new Vector3(size.x, -size.y, 0f)+Vector3.left* size.x);
        Debug.DrawLine(startPos + (size.y * Vector3.down), startPos);
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
