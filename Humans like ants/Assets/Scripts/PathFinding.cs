using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    public int humanId;
    Grid grid;
    public Vector3 destination;

    public void GiveNewTarget(Vector3 _target)
    {
        FindPath(_target, transform.position);
    }
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }
    private void Start()
    {
        StartCoroutine(MoveToDestination());
    }
    IEnumerator MoveToDestination()
    {
        
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(MoveToDestination());
    }
    void FindPath(Vector3 target, Vector3 start)
    {
        Cell targetCell = grid.CellFromWorldPosition(target);
        Cell origin = grid.CellFromWorldPosition(start);

        List<Cell> openList = new List<Cell>();
        HashSet<Cell> closedList = new HashSet<Cell>();

        openList.Add(targetCell);

        while (openList.Count > 0)
        {
            Cell currentNode = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Cell neighbornode in grid.GetNeighboringNodes(currentNode))
            {
                if (neighbornode.isWall || closedList.Contains(neighbornode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighbornode);
                if (moveCost < neighbornode.gCost || !openList.Contains(neighbornode))
                {
                    neighbornode.gCost = moveCost;
                    neighbornode.hCost = GetManhattenDistance(neighbornode, origin);
                    neighbornode.parent = currentNode;

                    if (!openList.Contains(neighbornode))
                    {
                        openList.Add(neighbornode);
                    }
                }
            }

            if (currentNode == origin)
            {
                GetFinalPath(targetCell, origin);
            }
        }
    }
    void GetFinalPath(Cell a_startingNode, Cell a_endNode)
    {
        List<Cell> finalPath = new List<Cell>();
        Cell currentNode = a_endNode;

        while (currentNode != a_startingNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        finalPath.Reverse();

        if (grid.pathList.Count > humanId)
        {
            FinalPaths haba;
            haba = new FinalPaths();
            haba.finalPath = new List<Cell>();
            grid.pathList[humanId] = haba;
            grid.pathList[humanId].finalPath.Clear();
            grid.pathList[humanId].finalPath.AddRange(finalPath);
            GetComponent<MoveToDestination>().MoveToNextCell();
        }
        else
        {
            grid.pathList.Add(new FinalPaths());
        }
    }
    int GetManhattenDistance(Cell a_nodeA, Cell a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return ix + iy;
    }
}
