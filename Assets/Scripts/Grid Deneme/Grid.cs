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
    GridNode[,] gridArray;
    int cellSize;
    public Grid(int xLen, int yLen, int cellSize, Transform transform)
    {
        gridStartPosition = CalculateStartPosForThisObject(transform);
        xLength = xLen;
        yLength = yLen;
        this.cellSize = cellSize;

        gridArray = new GridNode[xLen, yLen];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GridNode temp = new GridNode(x, y)
                {
                    fCost = 0,
                    hCost = 0,
                    gCost = 0
                };
                gridArray[x, y] = temp;
                // gridArray[x, y].number = new Vector2Int(x, y);
                //gridArray[x, y].number.x = x;
                //gridArray[x, y].number.y = y; 
                DrawCube(gridStartPosition + (cellSize * x * Vector3.right) + (cellSize * y * Vector3.up), cellSize);
            }
        }
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public Vector3 CalculateStartPosForThisObject(Transform transform)
    {
        return transform.position + new Vector3(-transform.localScale.x / 2, -transform.localScale.y / 2, 0);
    }

    public void DrawCube(Vector3 startPos, float size)
    {
        Color color = Color.white;
        Debug.DrawLine(startPos, startPos + Vector3.right * size, color, 100f);
        Debug.DrawLine(startPos + (size * Vector3.right), startPos + (size * Vector3.right) + Vector3.up * size, color, 100f);
        Debug.DrawLine(startPos + new Vector3(size, size, 0f), startPos + new Vector3(size, size, 0f) + Vector3.left * size, color, 100f);
        Debug.DrawLine(startPos, startPos + (size * Vector3.up), color, 100f);

        //Debug.DrawLine(startPos, startPos + size * Vector3.right);
        //Debug.DrawLine(startPos, startPos + size * Vector3.down);

    }

    public GridNode DefaultNode()
    {
        GridNode def = new()
        {
            hCost = 0,
            fCost = 0
        };
        return def;
    }
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < xLength && y < yLength)
        {
            gridArray[x, y].fCost = value;
        }
    }

    public void SetValue(Vector3 worldPos, int value)
    {
        GetXY(worldPos, out int x, out int y);
        SetValue(x, y, value);
    }

    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos.x - gridStartPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldPos.y - gridStartPosition.y) / cellSize);
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public float GetGridValue(int x, int y)
    {
        return gridArray[x, y].fCost;
    }

    public GridNode GetGridNumber(Vector3 worldPos)
    {
        GetXY(worldPos, out int x, out int y);
        Debug.LogWarning("Grid [" + x + "," + y + "]");
        return gridArray[x, y];
    }

    public List<GridNode> FindNeighbours(GridNode node)
    {
        List<GridNode> tempList = new List<GridNode>();
        int x = node.number.x;
        int y = node.number.y;

        if (y + 1 > 0) tempList.Add(FindNode(x, y + 1));
        if (x + 1 > 0 && y + 1 > 0)  tempList.Add(FindNode(x + 1, y + 1));
        if (x + 1 > 0) tempList.Add(FindNode(x + 1, y));
        if (x + 1 > 0 && y - 1 > 0) tempList.Add(FindNode(x + 1, y - 1));
        if (y - 1 > 0) tempList.Add(FindNode(x, y - 1));
        if (x - 1 > 0 && y - 1 > 0) tempList.Add(FindNode(x - 1, y - 1));
        if (x - 1 > 0) tempList.Add(FindNode(x - 1, y));
        if (x - 1 > 0 && y + 1 > 0) tempList.Add(FindNode(x - 1, y + 1));



        return tempList;
    }

    public GridNode FindNode(int x, int y)
    {
        return gridArray[x, y];
    }
}

[System.Serializable]
public class GridNode
{
    public Vector2Int number;
    public float hCost;
    public float gCost;
    public float fCost;

    public GridNode(int x = 0, int y = 0)
    {
        number.x = x;
        number.y = y;
    }

}



