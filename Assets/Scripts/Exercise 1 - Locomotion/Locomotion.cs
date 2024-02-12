using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{

    private Animator m_animator;
    private Tracker m_tracker;
    private OrientationManager m_orientationManager;
    private InputKeyManager m_inputKeyManager;

    public float velocityX = 0.0f;
    public float velocityZ = 0.0f;
    public Vector3 orientation;

    public float velocityToAnimationFidelityFactorZ = 3.5f;   // A public variable to align the character's Z position to the visual animation.
    public float velocityToAnimationFidelityFactorX = 3.0f;   // A public variable to align the character's X position to the visual animation.

    public float acceleration = 2.0f;
    public float deceleration = 4.0f;

    public float maximumWalkVelocity = 0.5f;    // In the blend tree, 0.5 is the walking threshold
    public float maximumRunVelocity = 2.0f;     // In the blend tree, 2.0 is the running threshold 

    // Increase performance
    int VelocityXHash;
    int VelocityZHash;

    // Key inputs
    private bool m_forwardPressed;
    private bool m_backwardPressed;
    private bool m_leftPressed;
    private bool m_rightPressed;
    private bool m_runPressed;
    private bool m_orientationFixPressed;

    // Start is called before the first frame update
    void Start()
    {
        // Get the gameobject's other important components
        m_animator = GetComponent<Animator>();
        m_tracker = GetComponent<Tracker>();
        m_orientationManager = GetComponent<OrientationManager>();
        m_inputKeyManager = GetComponent<InputKeyManager>();

        // Get the orientation from the Tracker
        orientation = m_tracker.orientation;

        // Increase performance
        VelocityXHash = Animator.StringToHash("Velocity X");
        VelocityZHash = Animator.StringToHash("Velocity Z");
    }

    // Handle acceleration and deceleration
    void changeVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        // Acceleration conditions
        // ---------------------------------------------------------------------
        // Set forward velocity
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;

        }

        // Set backward velocity
        if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;

        }

        // Set right velocity
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // Set left velocity
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }


        // Deceleration conditions
        // ---------------------------------------------------------------------
        // Decelerate moving forward
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        // Decelerate moving backward
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        // Decelerate moving right
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        // Decelerate moving left
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }



        // Move the character forward / backward
        if (forwardPressed || backwardPressed)
            transform.position += orientation * Time.deltaTime * velocityZ * velocityToAnimationFidelityFactorZ;
        // Move the character left / right
        if (leftPressed || rightPressed)
            transform.position += Quaternion.Euler(0, 90, 0) * orientation * Time.deltaTime * velocityX * velocityToAnimationFidelityFactorX;

    }

    // Handle reset and lock of velocity
    void lockOrResetVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        // Stopping conditions
        // ---------------------------------------------------------------------
        // Stop moving forward / Reset Velocity Z
        if (!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }

        // Stop moving left - right / Reset Velocity X
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }



        // Lock conditions
        // ---------------------------------------------------------------------

        // Forward
        // ---
        // Lock forward maximum velocity
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        // Backward
        // ---
        // Lock backward maximum velocity
        if (backwardPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (backwardPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.05f))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (backwardPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }

        // Right
        // ---
        // Lock right maximum velocity
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }

        // Left
        // ---
        // Lock left maximum velocity
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            // round to the currentMaxVelocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        // round to the currentMaxVelocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

    }


    // LateUpdate is called after all Update functions have been called
    // I need it to run as the "last" Update(), since it needs to get the proper current data first, and then do anything.
    void LateUpdate()
    {
        // Get Key Inputs from the Input Manager
        m_forwardPressed        = m_inputKeyManager.forwardPressed;
        m_backwardPressed       = m_inputKeyManager.backwardPressed;
        m_leftPressed           = m_inputKeyManager.leftPressed;
        m_rightPressed          = m_inputKeyManager.rightPressed;
        m_runPressed            = m_inputKeyManager.runPressed;
        m_orientationFixPressed = m_inputKeyManager.orientationFixPressed;

        // Toggle Fixed Orientation
        if (m_orientationFixPressed)
            m_orientationManager.ToggleOrientation();

        // Set current max velocity
        float currentMaxVelocity = m_runPressed ? maximumRunVelocity : maximumWalkVelocity;

        // Update the orientation based on movement
        orientation = m_tracker.orientation.normalized;

        // Handle the velocity
        changeVelocity(m_forwardPressed, m_backwardPressed, m_leftPressed, m_rightPressed, m_runPressed, currentMaxVelocity);
        lockOrResetVelocity(m_forwardPressed, m_backwardPressed, m_leftPressed, m_rightPressed, m_runPressed, currentMaxVelocity);

        // Update the animator's velocities
        m_animator.SetFloat(VelocityXHash, velocityX);
        m_animator.SetFloat(VelocityZHash, velocityZ);

    }

}
