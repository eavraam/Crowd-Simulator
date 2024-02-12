using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    private Tracker m_tracker;

    public bool isOrientationFixed = true;
    public float smoothingFactor = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        m_tracker = GetComponent<Tracker>();
    }

    private void LateUpdate()
    {
        // Modify the "forward" vector if the orientation is not fixed - Free moving
        if (!isOrientationFixed)
        {
            transform.forward = Vector3.Slerp(transform.forward, m_tracker.worldVelocity, smoothingFactor);
        }
    }

    public void ToggleOrientation()
    {
        isOrientationFixed = !isOrientationFixed;
    }
}
