using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Grid 
{
    public Vector3 startPos = Vector3.zero;
    int xLength;
    int yLength;
    int[,] gridArray;
    int cellLength;
    int cellHeight;
    public Grid(int xLen, int yLen,int cellLength,int cellHeight)
    {
        xLength = xLen;
        yLength = yLen;
        this.cellHeight = cellHeight;
        this.cellLength = cellLength;

        gridArray = new int[xLen, yLen];

        for (int i = 0; i < gridArray.GetLength(1);i++)
        {
            for (int j= 0; j < gridArray.GetLength(0); j++)
            {
                Debug.Log(i + " " + j);
            }
        }
    }

    public float GetCellLength()
    {
        return cellLength;
    }
    public float GetCellHeight()
    {
        return cellHeight;
    }
}
