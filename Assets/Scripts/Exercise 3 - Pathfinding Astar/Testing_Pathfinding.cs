using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Pathfinding : MonoBehaviour
{

    public GameObject obstaclePrefab;
    public int numberOfObstacles = 20;
    public Vector3 obstacleScale = new Vector3(10, 6, 10);
    public bool canWalkDiagonally = false;
    
    private Pathfinding pathfinding;


    private void Start()
    {
        pathfinding = new Pathfinding(10, 10);
        pathfinding.GetGrid().PlaceRandomObstacles(obstaclePrefab, numberOfObstacles, this.transform, obstacleScale);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXZ(mouseWorldPosition, out int x, out int z);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, z, canWalkDiagonally);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, 0, path[i].z) * 10.0f + new Vector3(5f, 0, 5f),
                    new Vector3(path[i + 1].x, 0, path[i + 1].z) * 10.0f + new Vector3(5f, 0, 5f),
                    Color.yellow,
                    3.0f);
                }
            }
        }

        // Debugging...
        // ------------
        if (Input.GetMouseButtonDown(1))
        {
            int tx, tz;
            pathfinding.GetGrid().GetXZ(GetMouseWorldPosition(), out tx, out tz);
            Debug.Log($"X: {tx}, Z: {tz}, Value: {pathfinding.GetGrid().GetGridObject(tx, tz)}");

            pathfinding.GetNode(tx, tz).SetIsWalkable(!pathfinding.GetNode(tx, tz).isWalkable);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
        vec.y = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithY(Vector3 screenPosition, Camera worldCamera)
    {
        // Adjust z to match the camera's height or another appropriate value
        float distanceToPlane = Mathf.Abs(worldCamera.transform.position.y - 0); // Assuming grid is at y = 0
        screenPosition.z = distanceToPlane;
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
