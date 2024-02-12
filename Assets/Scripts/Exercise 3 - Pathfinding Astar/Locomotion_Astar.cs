using UnityEngine;

public class Locomotion_Astar : MonoBehaviour
{
    private Animator m_animator;
    private PathManager_Astar m_pathManager;

    public float walkSpeed = 1.5f;
    public float turnSpeed = 5.0f;
    public float velocityToAnimationFidelityFactorZ = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_pathManager = GetComponent<PathManager_Astar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pathManager.direction.magnitude > 0.1f)
        {
            // Rotate towards the next node
            Quaternion targetRotation = Quaternion.LookRotation(m_pathManager.direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Move forward
            transform.Translate(Vector3.forward * walkSpeed * velocityToAnimationFidelityFactorZ * Time.deltaTime);

            // Animate walk
            m_animator.SetFloat("Velocity Z", walkSpeed);
        }
        else
        {
            // Stop animation when the character is not moving
            m_animator.SetFloat("Velocity Z", 0.0f);
        }
    }
}
