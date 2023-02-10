using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealisticDroneController : MonoBehaviour
{
    private GameData game_data;
    
    public GameObject propeller01;
    public GameObject propeller02;
    public GameObject propeller03;
    public GameObject propeller04;

    private Rigidbody drone_rig;

    public float power_multiplier;
    public float deceleration_multiplier;
    public float targetVelocity;

    public Slider PowerUI;
    public float takeOffPower;

    public Slider DetectUI;
    public float rotateGrePurPower;

    public float power01;
    public float power02;
    public float power03;
    public float power04;

    public float air_resistance;

    //public GameObject power01_bar;
    //public GameObject power02_bar;
    //public GameObject power03_bar;
    //public GameObject power04_bar;


    private bool isSlowMotion;

    private Vector3 init_pos;

    // Start is called before the first frame update
    void Start()
    {
        //game_data = GameObject.Find("GameData").GetComponent<GameData>();

        init_pos = transform.position;

        drone_rig = gameObject.GetComponent<Rigidbody>();

        isSlowMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        takeOffPower = PowerUI.value;
        rotateGrePurPower = DetectUI.value;
        //power01_bar.transform.localScale = new Vector3(1, power01, 1);
        //power02_bar.transform.localScale = new Vector3(1, power02, 1);
        //power03_bar.transform.localScale = new Vector3(1, power03, 1);
        //power04_bar.transform.localScale = new Vector3(1, power04, 1);

        if (Input.GetKey(KeyCode.R))
        {
            transform.rotation = new Quaternion();
            power01 = 0;
            power02 = 0;
            power03 = 0;
            power04 = 0;
            //propeller01.transform.rotation = new Quaternion();
            //propeller02.transform.rotation = new Quaternion();
            //propeller03.transform.rotation = new Quaternion();
            //propeller04.transform.rotation = new Quaternion();

            transform.position = game_data.Player_last_checkpoint.position;
            drone_rig.velocity = new Vector3();
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            transform.rotation = new Quaternion();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSlowMotion = !isSlowMotion;
            if (isSlowMotion)
            {
                Physics.gravity = Physics.gravity * 0.1f;
                drone_rig.velocity = drone_rig.velocity * 0.1f;
                drone_rig.angularVelocity = drone_rig.angularVelocity * 0.1f;
                power_multiplier *= 0.25f;
                deceleration_multiplier *= 0.25f;
            } else
            {
                Physics.gravity = Physics.gravity * 10f;
                drone_rig.velocity = drone_rig.velocity * 10f;
                drone_rig.angularVelocity = drone_rig.angularVelocity * 10f;
                power_multiplier *= 4;
                deceleration_multiplier *= 4;
            }
        }

        verticalTakeOff();
        rotateGrenPur();
    }

    private float power01_delta;
    private float power02_delta;
    private float power03_delta;
    private float power04_delta;
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (power01 == 0) power01 = 0.1f;
            power01 += power01_delta;

            power01_delta = power_multiplier / power01;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (power02 == 0) power02 = 0.1f;
            power02 += power02_delta;

            power02_delta = power_multiplier / power02;
        }

        if (Input.GetKey(KeyCode.K))
        {
            if (power03 == 0) power03 = 0.1f;
            power03 += power03_delta;

            power03_delta = power_multiplier / power03;
        }

        if (Input.GetKey(KeyCode.M))
        {
            if (power04 == 0) power04 = 0.1f;
            power04 += power04_delta;

            power04_delta = power_multiplier / power04;
        }


        //propeller01.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, power01_delta * 100));
        //propeller02.GetComponent<Rigidbody>().AddTorque(new Vector3(0, power02_delta * 10000 - propeller02.GetComponent<Rigidbody>().angularVelocity.z * 100, 0));
        //propeller03.GetComponent<Rigidbody>().AddTorque(new Vector3(0, power03_delta * 10000 - propeller03.GetComponent<Rigidbody>().angularVelocity.z * 100, 0));
        //propeller04.GetComponent<Rigidbody>().AddTorque(new Vector3(0, power04_delta * 10000 - propeller04.GetComponent<Rigidbody>().angularVelocity.z * 100, 0));

        propeller01.transform.Rotate(new Vector3(0, 0, power01 * 100f));
        propeller02.transform.Rotate(new Vector3(0, 0, -power02 * 100f));
        propeller03.transform.Rotate(new Vector3(0, 0, power03 * 100f));
        propeller04.transform.Rotate(new Vector3(0, 0, -power04 * 100f));
        
        
        // use delta rotation for better physics
        //drone_rig.AddForceAtPosition(propeller01.GetComponent<Rigidbody>().angularVelocity.z * propeller01.transform.forward, propeller01.transform.position);
        //drone_rig.AddForceAtPosition(propeller02.GetComponent<Rigidbody>().angularVelocity.z * propeller02.transform.forward, propeller02.transform.position);
        //drone_rig.AddForceAtPosition(propeller03.GetComponent<Rigidbody>().angularVelocity.z * propeller03.transform.forward, propeller03.transform.position);
        //drone_rig.AddForceAtPosition(propeller04.GetComponent<Rigidbody>().angularVelocity.z * propeller04.transform.forward, propeller04.transform.position);


        drone_rig.AddForceAtPosition(power01 * propeller01.transform.forward, propeller01.transform.position);
        drone_rig.AddForceAtPosition(power02 * propeller02.transform.forward, propeller02.transform.position);
        drone_rig.AddForceAtPosition(power03 * propeller03.transform.forward, propeller03.transform.position);
        drone_rig.AddForceAtPosition(power04 * propeller04.transform.forward, propeller04.transform.position);
        


        // power01 *= deceleration_multiplier;
        // if (power01 <= 0.01f) power01 = 0;
        // power02 *= deceleration_multiplier;
        // if (power02 <= 0.01f) power02 = 0;
        // power03 *= deceleration_multiplier;
        // if (power03 <= 0.01f) power03 = 0;
        // power04 *= deceleration_multiplier;
        // if (power04 <= 0.01f) power04 = 0;

        // air resistance
        //drone_rig.AddForce(-drone_rig.velocity * air_resistance);
    }

    public void verticalTakeOff()
    {
        // while (true)
        // {
        //     StartCoroutine("SlowlySpeedUp");
        // }
        power01 = takeOffPower;
        power02 = takeOffPower;
        power03 = takeOffPower;
        power04 = takeOffPower;
    }
    public void rotateGrenPur()
    {
        power01 += rotateGrePurPower;

        power04 -= rotateGrePurPower;
    }
    
    public void AutoEqual()
    {
        
    }
}
