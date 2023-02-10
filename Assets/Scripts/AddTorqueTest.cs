using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorqueTest : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rg;
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rg.AddTorque(gameObject.transform.up*10);
        //rg.AddTorque(0,0,0);
        
    }
}
