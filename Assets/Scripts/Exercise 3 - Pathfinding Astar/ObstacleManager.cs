using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int numberOfObstacles;
    public Vector3 obstacleScale;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 10.0f;

    private List<Vector3> obstaclePositions;

    void Start()
    {
        obstaclePositions = new List<Vector3>();
        PlaceRandomObstacles();
    }

    public List<Vector3> GetObstaclePositions()
    {
        return obstaclePositions;
    }

    private void PlaceRandomObstacles()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int randomX = Random.Range(1, gridWidth - 1);
            int randomZ = Random.Range(1, gridHeight - 1);

            Vector3 obstaclePosition = new Vector3(randomX * cellSize, 0, randomZ * cellSize) + new Vector3(cellSize / 2, 0, cellSize / 2);
            GameObject obstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity, transform);
            obstacle.transform.localScale = obstacleScale;

            obstaclePositions.Add(obstaclePosition);
        }
    }
}
