using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RealisticDroneController : MonoBehaviour
{
    [Header("===== Game Data =====")]
    private GameData game_data;
    
    [Header("===== Obj Setting =====")]
    public GameObject propeller01;
    public GameObject propeller02;
    public GameObject propeller03;
    public GameObject propeller04;

    private Rigidbody drone_rig;

    [Header("===== Power Controller =====")]
    //是否起飞
    public bool takeOffStata = true;
    public Toggle takeOffStataUI;
    
    //起飞控制
    public Slider takeOffPowerUI;
    public float takeOffPower;
    
    //均匀加速
    public float speedUpConstant;
    
    //对角旋转
    public bool rotateDiagonalStata = false;
    public Toggle rotateDiagonalStataUI;
    public Slider rotateDiagonalUI_1;
    public float rotateDiagonalPower_1;
    public Slider rotateDiagonalUI_2;
    public float rotateDiagonalPower_2;
    
    //相邻旋转
    public Slider rotateNeighborUI_1;
    public float rotateNeighborPower_1;
    public Slider rotateNeighborUI_2;
    public float rotateNeighborPower_2;
    
    [Header("===== Data Shown =====")]
    public float takeOffMax;
    public float rotateDiagonalMax;
    public float rotateNeighborMax;
    public float constantVelocityMax;
    
    public float power01;
    public float power02;
    public float power03;
    public float power04;
    
    public TMP_Text PowerShown;
    public TMP_Text HeightShown;
    public TMP_Text VelocityShown;

    public float air_resistance;
    
    private Vector3 init_pos;

    // Start is called before the first frame update
    void Start()
    {
        takeOffStata = true;
        rotateDiagonalStata = false;
        //game_data = GameObject.Find("GameData").GetComponent<GameData>();

        init_pos = transform.position;

        drone_rig = gameObject.GetComponent<Rigidbody>();

        takeOffPowerUI.maxValue = takeOffMax;
        rotateDiagonalUI_1.maxValue =  rotateDiagonalUI_2.maxValue= rotateDiagonalMax;
        rotateDiagonalUI_1.minValue = rotateDiagonalUI_2.minValue= -rotateDiagonalMax;
        rotateNeighborUI_1.maxValue =  rotateNeighborUI_2.maxValue= rotateNeighborMax;
        rotateNeighborUI_1.minValue = rotateNeighborUI_2.minValue= -rotateNeighborMax;

    }

    // Update is called once per frame
    
    void Update()
    {
        //RotateAddForce();
        
        HeightShown.text = drone_rig.position.y.ToString();
        VelocityShown.text = drone_rig.velocity.ToString();
        PowerShown.text = " A:" +Mathf.Round(power01*100).ToString()+"; B:" + Mathf.Round(power02*100).ToString()+"; C:" + Mathf.Round(power03*100).ToString()+"; D:" + Mathf.Round(power04*100).ToString();
        takeOffStataUI.isOn = takeOffStata;
        rotateDiagonalStataUI.isOn = rotateDiagonalStata;
        
        getJoyStickInput();
        
        takeOffPower = takeOffPowerUI.value;
        rotateDiagonalPower_1 = rotateDiagonalUI_1.value;
        rotateDiagonalPower_2 = rotateDiagonalUI_2.value;
        rotateNeighborPower_1 = rotateNeighborUI_1.value;
        rotateNeighborPower_2 = rotateNeighborUI_2.value;
        verticalTakeOff();
        SpeedUp();
        rotateDiagonal_1();
        rotateDiagonal_2();
        rotateNeighbor_1();
        rotateNeighbor_2();
    }
    public void getJoyStickInput()
    {
        // //是否切换为起飞状态,用按键切换
        // if (takeOffStata == false && JoyStickInputSetting._instance.ButtonX)
        // {
        //     takeOffStata = true;
        // }
        // else if(takeOffStata == true && JoyStickInputSetting._instance.ButtonX)
        // {
        //     takeOffStata = false;
        // }
        
        if (takeOffStata == true)
        {
            takeOffPowerUI.value = JoyStickInputSetting._instance.LeftVertical*takeOffPowerUI.maxValue;
        }
        else if(rotateDiagonalStata == true)
        {
            rotateDiagonalUI_1.value = JoyStickInputSetting._instance.LeftHorizontal*rotateDiagonalUI_1.maxValue;
            rotateDiagonalUI_2.value = JoyStickInputSetting._instance.LRT*rotateDiagonalUI_2.maxValue;
            
            speedUpConstant = JoyStickInputSetting._instance.LeftVertical;
            
            rotateNeighborUI_1.value = JoyStickInputSetting._instance.RightHorizontal*rotateNeighborUI_1.maxValue;
            rotateNeighborUI_2.value = JoyStickInputSetting._instance.RIghtVertical*rotateNeighborUI_2.maxValue;
        }
    }
    void FixedUpdate()
    {
        propeller01.transform.Rotate(new Vector3(0, 0, power01 * 100f));
        propeller02.transform.Rotate(new Vector3(0, 0, -power02 * 100f));
        propeller03.transform.Rotate(new Vector3(0, 0, power03 * 100f));
        propeller04.transform.Rotate(new Vector3(0, 0, -power04 * 100f));
        
        
        drone_rig.AddForceAtPosition(power01 * propeller01.transform.forward, propeller01.transform.position);
        drone_rig.AddForceAtPosition(power02 * propeller02.transform.forward, propeller02.transform.position);
        drone_rig.AddForceAtPosition(power03 * propeller03.transform.forward, propeller03.transform.position);
        drone_rig.AddForceAtPosition(power04 * propeller04.transform.forward, propeller04.transform.position);
        

    }

    void RotateAddForce()
    {
        propeller01.transform.Rotate(new Vector3(0, 0, power01 * 100f));
        propeller02.transform.Rotate(new Vector3(0, 0, -power02 * 100f));
        propeller03.transform.Rotate(new Vector3(0, 0, power03 * 100f));
        propeller04.transform.Rotate(new Vector3(0, 0, -power04 * 100f));
        

        drone_rig.AddForceAtPosition(power01 * propeller01.transform.forward, propeller01.transform.position);
        drone_rig.AddForceAtPosition(power02 * propeller02.transform.forward, propeller02.transform.position);
        drone_rig.AddForceAtPosition(power03 * propeller03.transform.forward, propeller03.transform.position);
        drone_rig.AddForceAtPosition(power04 * propeller04.transform.forward, propeller04.transform.position);
    }
    
    public void verticalTakeOff()
    {
        if (takeOffStata == true)
        {
            power01 = takeOffPower;
            power02 = takeOffPower;
            power03 = takeOffPower;
            power04 = takeOffPower;
            if (power01 + power02 + power03 + power04 >= drone_rig.mass * 9.81f && Math.Abs(drone_rig.velocity.y) >=constantVelocityMax && rotateDiagonalStata == false)
            {
                takeOffStata = false;
                rotateDiagonalStata = true;
                power01 = power02 = power03 = power04 = drone_rig.mass * 9.81f / 4;
            }
        }
    }
    public void rotateDiagonal_1()
    {
        float tempPower;
        tempPower = rotateDiagonalPower_1;
        power01 += tempPower;

        power04 -= tempPower;
        StartCoroutine("AutoFixBackDiagonal_1",tempPower);
    }
    public void rotateDiagonal_2()
    {
        float tempPower;
        tempPower = rotateDiagonalPower_2;
        power02 += tempPower;

        power03 -= tempPower;
        StartCoroutine("AutoFixBackDiagonal_2",tempPower);
    }

    IEnumerator AutoFixBackDiagonal_1(float tempRotateDiagonalPower_1)
    {
        yield return new WaitForSeconds(0.001f);
        power01 -= tempRotateDiagonalPower_1;

        power04 += tempRotateDiagonalPower_1;
    }
    IEnumerator AutoFixBackDiagonal_2(float tempRotateDiagonalPower_2)
    {
        yield return new WaitForSeconds(0.001f);
        power01 -= tempRotateDiagonalPower_2;
        power04 += tempRotateDiagonalPower_2;
    }
    
    public void SpeedUp()
    {
        float tempPower;
        tempPower = speedUpConstant;
        power01 += tempPower;
        power02 += tempPower;
        power04 += tempPower;
        power03 += tempPower;
        StartCoroutine("AutoFixBackSpeedUp",tempPower);
    }
    IEnumerator AutoFixBackSpeedUp(float tempSpeedUpPower)
    {
        yield return new WaitForSeconds(0.001f);
        power01 -= tempSpeedUpPower;
        power02 -= tempSpeedUpPower;
        power04 -= tempSpeedUpPower;
        power03 -= tempSpeedUpPower;
    }
    
    public void rotateNeighbor_1()
    {
        float tempPower;
        tempPower = rotateNeighborPower_1;
        power01 += tempPower;
        power03 += tempPower;
        StartCoroutine("AutoFixBackNeighbor_1",tempPower);
    }
    public void rotateNeighbor_2()
    {
        float tempPower;
        tempPower = rotateNeighborPower_2;
        power02 -= tempPower;
        power01 -= tempPower;
        StartCoroutine("AutoFixBackNeighbor_2",tempPower);
    }
    
    IEnumerator AutoFixBackNeighbor_1(float tempRotateNeighborPower_1)
    {
        yield return new WaitForSeconds(0.0001f);
        power01 -= tempRotateNeighborPower_1;
        power03 -= tempRotateNeighborPower_1;
    }
    
    IEnumerator AutoFixBackNeighbor_2(float tempRotateNeighborPower_2)
    {
        yield return new WaitForSeconds(0.0001f);
        power02 += tempRotateNeighborPower_2;
        power01 += tempRotateNeighborPower_2;
    }
}
