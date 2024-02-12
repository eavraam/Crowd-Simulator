using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;   // Queuing up for searching
    private List<PathNode> closedList; // Already searched
    private bool m_canWalkDiagonally;

    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10.0f, Vector3.zero,
            (Grid<PathNode> g, int x, int z) => new PathNode(grid, x, z));
    }

    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ, bool canWalkDiagonally)
    {
        PathNode startNode = grid.GetGridObject(startX, startZ);
        PathNode endNode = grid.GetGridObject(endX, endZ);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        m_canWalkDiagonally = canWalkDiagonally;

        // Set the gCost of every Node to infinite and calculate the fCost
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z= 0; z < grid.GetHeight(); z++)
            {
                PathNode pathNode = grid.GetGridObject(x, z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Node handling loop
        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode) // Reached final node
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Cycle through all the neighbors
            foreach (PathNode neighborNode in GetNeighborList(currentNode))
            {
                // Move forward if we already searched neighbor
                if (closedList.Contains(neighborNode)) continue;
                // Check if the neighbor is walkable
                if (!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }
                // Check if we have a faster path from the current to neighbor than we had previously
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    // Add to openList, if not already there
                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>(); 

        if (currentNode.x - 1 >= 0) // if isValid
        {
            // Left
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.z));
            // Diagonal neighbors
            if (m_canWalkDiagonally)
            {
                // Left Down
                if (currentNode.z - 1 >= 0) { neighborList.Add(GetNode(currentNode.x - 1, currentNode.z - 1)); }
                // Left Up
                if (currentNode.z + 1 < grid.GetHeight()) { neighborList.Add(GetNode(currentNode.x - 1, currentNode.z + 1)); }
            }
        }
        if (currentNode.x + 1 < grid.GetWidth()) // if isValid
        {
            // Right
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.z));
            // Diagonal neighbors
            if (m_canWalkDiagonally)
            {
                // Right Down
                if (currentNode.z - 1 >= 0) { neighborList.Add(GetNode(currentNode.x + 1, currentNode.z - 1)); }
                // Right Up
                if (currentNode.z + 1 < grid.GetHeight()) { neighborList.Add(GetNode(currentNode.x + 1, currentNode.z + 1)); }
            }    
        }
        // Down
        if (currentNode.z - 1 >= 0) { neighborList.Add(GetNode(currentNode.x, currentNode.z - 1)); }
        // Up
        if (currentNode.z + 1 < grid.GetHeight()) { neighborList.Add(GetNode(currentNode.x, currentNode.z + 1)); }

        return neighborList;
    }

    public PathNode GetNode(int x, int z)
    {
        return grid.GetGridObject(x, z);
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}
