using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFollow : MonoBehaviour
{
    public GameObject followedObj;
    
    // Update is called once per frame
    private void Update()
    {
        //transform.localPosition = Vector3.Lerp(transform.localPosition,followedObj.transform.localPosition,Time.deltaTime*100) ;
        transform.localPosition = followedObj.transform.localPosition ;
        transform.eulerAngles = new Vector3(0f, (float)followedObj.transform.rotation.eulerAngles.y,0f);
        //print("Tits' forward: "+transform.forward.ToString());
        //print("drones euler: "+followedObj.transform.rotation.eulerAngles);
    }
}
