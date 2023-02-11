using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForce : MonoBehaviour
{

    public Rigidbody rigi;
    public Vector3 torqueValue;

    public float forceValue;
    
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody>();
        rigi.AddForce(torqueValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            torqueValue = new Vector3(0, forceValue, 0);
            rigi.AddForce(torqueValue);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            torqueValue = new Vector3(0, -forceValue, 0);
            rigi.AddForce(torqueValue);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            torqueValue = new Vector3(forceValue, 0, 0);
            rigi.AddForce(torqueValue);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            torqueValue = new Vector3(-forceValue, 0, 0);
            rigi.AddForce(torqueValue);
        }
    }
}
