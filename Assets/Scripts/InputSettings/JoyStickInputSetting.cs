using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInputSetting : MonoBehaviour
{
    public float LeftHorizontal;
    public float LeftVertical;
    public float RightHorizontal;
    public float RIghtVertical;
    public float LRT;
    public bool ButtonX;
    
    public string input_LeftHorizontal;
    public string input_LeftVertical;
    public string input_RightHorizontal;
    public string input_RIghtVertical;
    public string input_LRT;
    public string input_ButtomX;

    public static JoyStickInputSetting _instance;
    // Start is called before the first frame update
    public void Awake()
    {
        _instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        LeftHorizontal = Input.GetAxis(input_LeftHorizontal);
        LeftVertical = Input.GetAxis(input_LeftVertical);
        RightHorizontal = Input.GetAxis(input_RightHorizontal);
        RIghtVertical = Input.GetAxis(input_RIghtVertical);
        LRT = Input.GetAxis(input_LRT);
        ButtonX = Input.GetButtonDown(input_ButtomX);
    }
}
