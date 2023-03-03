using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControllerPID : MonoBehaviour
{
    public float previousError;

    public float errorIntegral;

    public Transform settedPoint;

    public Transform thisPoint;

    public float Kp;

    public float Ki;

    public float Kd;
    // Start is called before the first frame update
    void Start()
    {
        thisPoint = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float error = settedPoint.position.y - transform.position.y;
        print("error: "+error.ToString());
        errorIntegral += error * Time.fixedDeltaTime;
        float derivative = (error - previousError) / Time.fixedDeltaTime;
        float deltaMove = Kp * error + Ki * errorIntegral + Kd * derivative;
        previousError = error;
        print("derivative: " + derivative.ToString());
        print("deltaMove: "+deltaMove.ToString());
        transform.position = new Vector3(transform.position.x, transform.position.y + deltaMove, transform.position.z);
    }
}
