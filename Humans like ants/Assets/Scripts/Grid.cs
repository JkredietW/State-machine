using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FinalPaths
{
    public List<Cell> finalPath;
}

public class Grid : MonoBehaviour
{
    public Vector2 gridSize;
    public float cellSize;
    public LayerMask wallMask;

    public List<FinalPaths> pathList;
    Cell[,] grid;

    private float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = cellSize * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        HumanBehaviour[] allHumans = FindObjectsOfType<HumanBehaviour>();
        //pathList = new List<FinalPaths>(10);

        GenerateGrid();
    }
    public void GenerateGrid()
    {
        grid = new Cell[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeY; z++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + cellSize) + Vector3.forward * (z * nodeDiameter + cellSize);
                bool wall = false;
                if (Physics.CheckSphere(worldPoint, cellSize, wallMask))
                {
                    wall = true;
                }
                grid[x, z] = new Cell(wall, worldPoint, x, z);
            }
        }
    }
    public Cell CellFromWorldPosition(Vector3 a_worldPosition)
    {
        float xpoint = (a_worldPosition.x + gridSize.x / 2) / gridSize.x;
        float zpoint = (a_worldPosition.z + gridSize.y / 2) / gridSize.y;

        xpoint = Mathf.Clamp01(xpoint);
        zpoint = Mathf.Clamp01(zpoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * zpoint);

        return grid[x, y];
    }
    public List<Cell> GetNeighboringNodes(Cell a_node)
    {
        List<Cell> neighboringNodes = new List<Cell>();
        int xCheck;
        int yCheck;

        //right side
        xCheck = a_node.gridX + 1;
        yCheck = a_node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //left side
        xCheck = a_node.gridX - 1;
        yCheck = a_node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //top side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //bottom side
        xCheck = a_node.gridX;
        yCheck = a_node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighboringNodes;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));
        if (grid != null)
        {
            foreach (Cell cell in grid)
            {
                if (!cell.isWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {

                    Gizmos.color = Color.yellow;
                }

                for (int i = 0; i < pathList.Count; i++)
                {
                    if (pathList[i].finalPath != null)
                    {
                        if (pathList[i].finalPath.Contains(cell))
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                }
                Gizmos.DrawCube(cell.position, Vector3.one * (nodeDiameter));
            }
        }
    }
}
