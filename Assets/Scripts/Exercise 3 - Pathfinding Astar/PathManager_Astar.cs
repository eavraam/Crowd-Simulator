using System.Collections.Generic;
using UnityEngine;

public class PathManager_Astar : MonoBehaviour
{
    public float distanceThreshold = 2.0f;
    public float minWorldLimit;
    public float maxWorldLimit;
    public bool canWalkDiagonally = false;

    public Vector3 direction; // Direction towards the next node

    private Pathfinding pathfinding;
    private List<PathNode> currentPath;
    private int pathIndex;
    private PathNode currentGoalNode;

    void Start()
    {
        pathfinding = new Pathfinding(10, 10);

        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        if (obstacleManager != null)
        {
            UpdateGridObstacles(obstacleManager.GetObstaclePositions());
        }

        SetNewGoal();
    }

    void Update()
    {
        if (currentPath != null && pathIndex < currentPath.Count)
        {
            PathNode nextNode = currentPath[pathIndex];
            Vector3 nextNodePosition = GetCenteredWorldPosition(nextNode.x, nextNode.z);

            direction = nextNodePosition - transform.position; // Update direction

            if (direction.magnitude < distanceThreshold)
            {
                pathIndex++;
                if (pathIndex >= currentPath.Count)
                {
                    SetNewGoal();
                }
            }
        }
    }

    void SetNewGoal()
    {
        float cellSize = pathfinding.GetGrid().GetCellSize();
        float halfCellSize = cellSize / 2f;

        do
        {
            // Randomly choose a cell
            int cellX = Random.Range(0, pathfinding.GetGrid().GetWidth());
            int cellZ = Random.Range(0, pathfinding.GetGrid().GetHeight());

            // Calculate the world position at the center of the cell
            Vector3 centerOfCell = new Vector3(
                cellX * cellSize + halfCellSize,
                0,
                cellZ * cellSize + halfCellSize
            );

            pathfinding.GetGrid().GetXZ(centerOfCell, out int endX, out int endZ);
            currentGoalNode = pathfinding.GetNode(endX, endZ);
            UpdatePath(currentGoalNode);
        } while (currentPath == null || currentPath.Count == 0);

        pathIndex = 0;
    }


    void UpdatePath(PathNode newGoalNode)
    {
        pathfinding.GetGrid().GetXZ(transform.position, out int startX, out int startZ);
        currentPath = pathfinding.FindPath(startX, startZ, newGoalNode.x, newGoalNode.z, canWalkDiagonally);
    }

    private Vector3 GetCenteredWorldPosition(int x, int z)
    {
        float cellSize = pathfinding.GetGrid().GetCellSize();
        float halfCellSize = cellSize / 2f;

        Vector3 worldPosition = pathfinding.GetGrid().GetWorldPosition(x, z);
        worldPosition.x += halfCellSize;
        worldPosition.z += halfCellSize;

        return worldPosition;
    }

    public void UpdateGridObstacles(List<Vector3> obstaclePositions)
    {
        foreach (var obstaclePosition in obstaclePositions)
        {
            pathfinding.GetGrid().GetXZ(obstaclePosition, out int x, out int z);
            PathNode node = pathfinding.GetNode(x, z);
            if (node != null)
            {
                node.SetIsWalkable(false);
            }
        }
    }

    // Visual debugging of the Agent's goals and directions
    private void OnDrawGizmos()
    {
        if (currentPath != null && currentPath.Count > 0 && pathIndex < currentPath.Count)
        {
            Gizmos.color = Color.green;
            Vector3 currentNodePosition = transform.position;
            for (int i = pathIndex; i < currentPath.Count; i++)
            {
                Vector3 nodePosition = GetCenteredWorldPosition(currentPath[i].x, currentPath[i].z);
                Gizmos.DrawLine(currentNodePosition, nodePosition);
                currentNodePosition = nodePosition;
            }
        }

        // Draw the direction line
        if (direction != Vector3.zero)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }

        // Draw the current goal at the center of its cell
        if (currentGoalNode != null)
        {
            Vector3 goalPosition = GetCenteredWorldPosition(currentGoalNode.x, currentGoalNode.z);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(goalPosition, 0.5f);
        }
    }


}
