using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid<TGridObject>
{

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private Vector3 m_originPosition;
    public TGridObject[,] gridArray;

    // Debugs
    float debug_grid_duration = 100.0f; // Grid debugging lasts for 100 sec

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;
        m_originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }


        //// Debugging...
        //// ----------
        //for (int x = 0; x < gridArray.GetLength(0); x++)
        //{
        //    for (int z = 0; z < gridArray.GetLength(1); z++)
        //    {
        //        //Debug.Log(x + "," + z); 
        //        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, debug_grid_duration);
        //        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, debug_grid_duration);
        //    }
        //}
        //// Complete the debugs for top and right
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, debug_grid_duration);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, debug_grid_duration);

    }

    public int GetWidth()
    {
        return m_width;
    }

    public int GetHeight()
    {
        return m_height;
    }

    public float GetCellSize()
    {
        return m_cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * m_cellSize + m_originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - m_originPosition).x / m_cellSize);
        z = Mathf.FloorToInt((worldPosition - m_originPosition).z / m_cellSize);
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && x < m_width && z >= 0 && z < m_height)
        {
            gridArray[x, z] = value;
            if (OnGridObjectChanged != null)
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        if (OnGridObjectChanged != null)
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && x < m_width && z >= 0 && z < m_height)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    // Method to place obstacles
    public void PlaceRandomObstacles(GameObject obstaclePrefab, int numberOfObstacles, Transform parentTransform, Vector3 obstacleScale)
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int randomX = UnityEngine.Random.Range(1, m_width-1);
            int randomZ = UnityEngine.Random.Range(1, m_height-1);

            TGridObject gridObject = GetGridObject(randomX, randomZ);

            if (gridObject is PathNode pathNode && pathNode.isWalkable)
            {
                Vector3 obstaclePosition = GetWorldPosition(randomX, randomZ);
                // Offset to center the obstacle in the middle of the grid cell
                obstaclePosition += new Vector3(m_cellSize / 2, 0, m_cellSize / 2);
                GameObject obstacle = UnityEngine.Object.Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity, parentTransform);
                obstacle.transform.localScale = obstacleScale;

                pathNode.SetIsWalkable(false);
            }
        }
    }

}
