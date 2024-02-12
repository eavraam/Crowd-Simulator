using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    private Grid<int> grid;
    private Vector3 gridOrigin;

    void Start()
    {
        gridOrigin = new Vector3 (0, 0, 0);
        grid = new Grid<int>(10, 10, 10.0f, gridOrigin, (Grid<int> g, int x, int z) => 0);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetGridObject(GetMouseWorldPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1))
        {
            int tx, tz;
            //Debug.Log(grid.GetValue(GetMouseWorldPosition()));
            grid.GetXZ(GetMouseWorldPosition(), out tx, out tz);
            Debug.Log("X: " + tx + ", Z: " + tz + ", Value: " + grid.gridArray[tx, tz]);
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
