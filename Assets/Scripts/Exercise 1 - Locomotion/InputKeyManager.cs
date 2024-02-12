using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyManager : MonoBehaviour
{

    // Key inputs
    [HideInInspector]
    public bool forwardPressed;
    [HideInInspector]
    public bool backwardPressed;
    [HideInInspector]
    public bool leftPressed;
    [HideInInspector]
    public bool rightPressed;
    [HideInInspector]
    public bool runPressed;
    [HideInInspector]
    public bool orientationFixPressed;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize orientation as fixed
        orientationFixPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Key Inputs
        forwardPressed = Input.GetKey(KeyCode.W);
        backwardPressed = Input.GetKey(KeyCode.S);
        leftPressed = Input.GetKey(KeyCode.A);
        rightPressed = Input.GetKey(KeyCode.D);
        runPressed = Input.GetKey(KeyCode.LeftShift);
        orientationFixPressed = Input.GetKeyDown(KeyCode.O); // KeyDown to handle once
    }
}
