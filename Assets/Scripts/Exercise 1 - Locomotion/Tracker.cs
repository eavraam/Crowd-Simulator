using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Tracker : MonoBehaviour
{
    // Gameobject's general-purpose data
    private Transform m_transform;
    private Vector3 m_position;
    public Vector3 orientation;

    // Displacements
    [SerializeField]
    private Vector3 m_prevPosition;
    [SerializeField]
    private Vector3 m_worldDisplacement;

    // Speed
    public Vector3 worldVelocity;
    public Vector3 localVelocity;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize general parameters
        m_transform = GetComponent<Transform>();
        m_position = transform.position;
        orientation = transform.forward;

        // Initialize displacements
        m_prevPosition = transform.position;    // Initialize it as equal for starters
        m_worldDisplacement = Vector3.zero;

        // Initialize velocities
        worldVelocity = Vector3.zero;
    }

    // Frame-rate independent Update function for physics calculations
    void FixedUpdate()
    {
        // Update the previous position
        m_prevPosition = m_position;

        // Update the general data
        m_position = m_transform.position;
        orientation = m_transform.forward;

        // Update the displacements
        m_worldDisplacement = m_position - m_prevPosition;

        // Update the velocities
        worldVelocity = m_worldDisplacement / Time.deltaTime;
        localVelocity = transform.InverseTransformDirection(localVelocity);

        bool resetPressed = Input.GetKey(KeyCode.R);

        // Reset
        if (resetPressed)
        {
            worldVelocity = Vector3.zero;
            localVelocity = Vector3.zero;
            m_worldDisplacement = Vector3.zero;
        }
    }


    // Visual debugging of the Tracker's data
    private void OnDrawGizmos()
    {
        // Forward / orientation line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_position + Vector3.up * 1.7f, m_position + orientation + Vector3.up * 1.7f);

        // World displacement line
        Gizmos.color = Color.green;
        Gizmos.DrawLine(m_prevPosition + Vector3.up * 0.5f, m_position + Vector3.up * 0.5f);

        // World velocity line
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(m_position + Vector3.up, m_position + worldVelocity + Vector3.up);
    }
}
