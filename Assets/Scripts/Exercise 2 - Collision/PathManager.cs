using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Vector3 goal;
    public float distanceThreshold = 2.0f;
    public float minWorldLimit;
    public float maxWorldLimit;

    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        // Generate a random initial goal to the agent
        Vector3 m_tempGoal = new Vector3(
            Random.Range(minWorldLimit, maxWorldLimit),
            0,
            Random.Range(minWorldLimit, maxWorldLimit)
        );

        // Assign the initial goal to the agent
        goal = m_tempGoal;
    }

    // Update is called once per frame
    void Update()
    {
        // Compute the direction of the agent towards its goal
        direction = goal - transform.position;

        // If the goal is reached (within threshold), generate a new goal
        if (direction.magnitude < distanceThreshold)
        {
            Vector3 m_tempGoal = new Vector3(
                Random.Range(minWorldLimit, maxWorldLimit),
                0,
                Random.Range(minWorldLimit, maxWorldLimit)
            );

            // Assign the new goal to the agent
            goal = m_tempGoal;
        }
    }

    // Visual debugging of the Agent's goals and directions
    private void OnDrawGizmos()
    {
        // Distance-to-goal line
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, goal);

        // Goal (as sphere)
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(goal, 1.0f);

    }

}
