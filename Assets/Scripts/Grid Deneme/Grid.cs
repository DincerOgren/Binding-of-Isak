using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using CodeMonkey.Utils;

public class Grid
{
    public Vector3 gridStartPosition = Vector3.zero;

    int xLength;
    int yLength;
    int[,] gridArray;
    int cellSize;
    public Grid(int xLen, int yLen, int cellSize,Transform transform)
    {
        gridStartPosition=  CalculateStartPosForThisObject(transform);
        xLength = xLen;
        yLength = yLen;
        this.cellSize = cellSize;

        gridArray = new int[xLen, yLen];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                DrawCube(gridStartPosition + (cellSize * x * Vector3.right) + (cellSize * y * Vector3.up), cellSize, GetGridValue(x, y));
            }
        }
        SetValue(1, 1, 1);
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public Vector3 CalculateStartPosForThisObject(Transform transform)
    {
        return  transform.position + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
    }

    public void DrawCube(Vector3 startPos, float size,int value)
    {
        Color color = Color.cyan;
        if (value == 0)
        {
            color = Color.white;
        }
        else if(value == 1)
        {
            color = Color.red;
        }
        Debug.DrawLine(startPos, startPos + Vector3.right * size,color,100f);
        Debug.DrawLine(startPos + (size * Vector3.right), startPos + (size * Vector3.right) + Vector3.up * size, color, 100f);
        Debug.DrawLine(startPos + new Vector3(size, size, 0f), startPos + new Vector3(size, size, 0f) + Vector3.left * size, color, 100f);
        Debug.DrawLine(startPos, startPos+(size*Vector3.up), color, 100f);

        //Debug.DrawLine(startPos, startPos + size * Vector3.right);
        //Debug.DrawLine(startPos, startPos + size * Vector3.down);

    }

    public void SetValue(int x,int y,int value)
    {
        if (x >= 0 && y >= 0 && x < xLength && y < yLength)
        {
            gridArray[x, y] = value;
        }
    }
    
    public void SetValue(Vector3 worldPos,int value)
    {
        GetXY(worldPos, out int x, out int y);
        SetValue(x, y, value);
    }

    private void GetXY(Vector3 worldPos,out int x,out int y)
    {
        x=Mathf.FloorToInt((worldPos.x - gridStartPosition.x )/ cellSize);
        y = Mathf.FloorToInt((worldPos.y - gridStartPosition.y) / cellSize);
    }
    public Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public int GetGridValue(int x,int y)
    {
        return gridArray[x, y];
    }

}
