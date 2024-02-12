using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    public GameObject prop;

    public float minWorldLimit = 0;
    public float maxWorldLimit = 100;

    public float cellSize = 10.0f; // Size of a cell

    public int numberOfAgents = 50;
    private List<Vector3> m_agentPositions;

    private float m_minDistance;
    private HashSet<Vector3> obstaclePositions;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize my agents' position list
        m_agentPositions = new List<Vector3>();

        // Initialize obstaclePositions as an empty HashSet
        obstaclePositions = new HashSet<Vector3>();

        // Define the minimum possible instantiation distance
        m_minDistance = prop.transform.localScale.y;

        // Get the obstacle positions from ObstacleManager
        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        if (obstacleManager != null)
        {
            obstaclePositions = new HashSet<Vector3>(obstacleManager.GetObstaclePositions());
        }

        // Randomly generate #numOfAgents agents
        for (int i = 0; i < numberOfAgents; i++)
        {
            Vector3 position = GenerateRandomPosition(m_agentPositions);  // Random position
            Quaternion rotation = Quaternion.Euler(0, 0, 0);              // Random rotation

            // Instantiate agent
            GameObject agent = Instantiate(prop, position, rotation);

            // Set the parent of the instantiated props to be this CrowdGenerator
            agent.transform.parent = transform;
        }

    }

    // Random Position Generator
    private Vector3 GenerateRandomPosition(List<Vector3> takenPositions)
    {
        Vector3 position;
        do
        {
            int gridWidth = Mathf.FloorToInt((maxWorldLimit - minWorldLimit) / cellSize);
            int gridHeight = gridWidth; // Assuming a square grid

            int cellX = Random.Range(0, gridWidth);
            int cellZ = Random.Range(0, gridHeight);

            float worldX = (cellX * cellSize) + minWorldLimit + (cellSize / 2);
            float worldZ = (cellZ * cellSize) + minWorldLimit + (cellSize / 2);
            position = new Vector3(worldX, 1, worldZ);
        } while (IsPositionTooClose(position, takenPositions) || obstaclePositions.Contains(position));

        return position;
    }

    // Check if the position is too close to any existing positions
    bool IsPositionTooClose(Vector3 position, List<Vector3> existingPositions)
    {
        foreach (Vector3 existingPosition in existingPositions)
        {
            if (Vector3.Distance(position, existingPosition) < m_minDistance)
            {
                return true; // Too close, generate a new position
            }
        }
        return false; // Position is fine
    }

}
