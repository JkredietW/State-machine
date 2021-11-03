using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int gridX;
    public int gridY;

    public bool isWall;
    public Vector3 position;

    public Cell parent;

    public int gCost, hCost;
    public int FCost { get { return gCost + hCost; } }

    public Cell(bool _isWall, Vector3 _position, int _gridX, int _gridY)
    {
        gridX = _gridX;
        gridY = _gridY;
        position = _position;
        isWall = _isWall;
    }
}
