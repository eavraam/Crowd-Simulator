using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float maxSpeed = 2.0f;   // Also max velocity of Character
    public float radius = 0.5f;     // Default is 0.5f, info taken from Capsule Collider Radius
    public Vector3 velocity;        // Magnitude indicates speed

    private PathManager m_pathManager;
    private Rigidbody m_rb;
    //private Vector3 m_updatedPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_pathManager = GetComponent<PathManager>();
    }

    // Frame-rate independent Update function for physics calculations
    void FixedUpdate()
    {
        velocity = m_pathManager.direction.normalized * maxSpeed;   // Compute agent's velocity
        m_rb.position += Time.deltaTime * velocity;                 // Update agent's position
        
        // another way
        //m_updatedPosition = m_rb.position + Time.deltaTime * velocity;
        //m_rb.MovePosition(m_updatedPosition);
    }
}
