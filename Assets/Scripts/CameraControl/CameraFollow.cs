using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followedObj;
    
    // Update is called once per frame
    private void Update()
    {
        //transform.localPosition = Vector3.Lerp(transform.localPosition,followedObj.transform.localPosition,Time.deltaTime*100) ;
        transform.localPosition = followedObj.transform.localPosition ;
    }
}
