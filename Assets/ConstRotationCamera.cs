using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstRotationCamera : MonoBehaviour
{
    private Vector3 oriEularRotation;
    // Start is called before the first frame update
    void Start()
    {
        oriEularRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(oriEularRotation);
    }
    
    
}
