using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInputSetting : MonoBehaviour
{
    public bool startKeyControl;
    public bool endKeyControl;
    
    public bool takeOff;
    public bool descent;

    public bool rotateRight;
    public bool rotateLeft;

    public bool moveRight;
    public bool moveLeft;
    
    public bool moveFront;
    public bool moveBack;

    public string inputStartControl;
    public string inputEndControl;
    
    public string inputTakeOff;
    public string inputDescent;
    
    public string inputRotateRight;
    public string inputRotateLeft;
    
    public string inputMoveRight;
    public string inputMoveLeft;
    
    public string inputMoveFront;
    public string inputMoveBack;
    
    public static KeyBoardInputSetting _instance;
    
    public void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        StartTheControl();
        TakeOffDescent();
        rotateRight = Input.GetKeyDown(inputRotateRight);
        rotateLeft = Input.GetKeyDown(inputRotateLeft);
        Move();
    }

    void StartTheControl()
    {
        if (Input.GetKeyDown(inputStartControl))
        {
            startKeyControl = !startKeyControl;
        }
        if (Input.GetKeyDown(inputEndControl))
        {
            endKeyControl = !endKeyControl;
        }
    }

    void TakeOffDescent()
    {
        if (Input.GetKeyDown(inputTakeOff))
        {
            takeOff = true;
        }
        if (Input.GetKeyDown(inputDescent))
        {
            descent = true;
        }
    }

    void Move()
    {
        if (Input.GetKeyDown(inputMoveRight))
        {
            moveRight = true;
        }
        if (Input.GetKeyDown(inputMoveLeft))
        {
            moveLeft = true;
        }
        if (Input.GetKeyDown(inputMoveFront))
        {
            moveFront = true;
        }
        if (Input.GetKeyDown(inputMoveBack))
        {
            moveBack = true;
        }
    }
    
}
