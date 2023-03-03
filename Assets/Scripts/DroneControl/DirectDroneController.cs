using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectDroneController : MonoBehaviour
{
    public Rigidbody droneRigi;


    public bool onSky;
    public GameObject tits;
    public GameObject propeller01;
    public GameObject propeller02;
    public GameObject propeller03;
    public GameObject propeller04;
    
    public TMP_Text VelocityShown;
    public TMP_Text HeightShown;
    
    // Start is called before the first frame update
    void Start()
    {
        droneRigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onSky)
        {
             rotateDroneFake();
        }
        VelocityShown.text = droneRigi.velocity.ToString();
        HeightShown.text = transform.position.y.ToString();
        if (KeyBoardInputSetting._instance.startKeyControl)
        {
            if (KeyBoardInputSetting._instance.takeOff)
            {
                onSky = true;
                droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,10f*tits.transform.up,Time.deltaTime) ;
                StartCoroutine("TakeOffHovering");
            }
            if (KeyBoardInputSetting._instance.descent)
            {
                if (transform.position.y >= 0.5f)
                {
                    droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,-10f*tits.transform.up,Time.deltaTime) ;
                }
                else
                {
                    onSky = false;
                    StartCoroutine("DescentHovering");
                }
            }
            if (KeyBoardInputSetting._instance.moveFront)
            {
                droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,10f*tits.transform.forward,Time.deltaTime*10f) ;
                //droneRigi.velocity = 10f*tits.transform.forward;
                //transform.rotation = Quaternion.Euler(30f, transform.rotation.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(30f, transform.rotation.eulerAngles.y, 0f),Time.deltaTime*10f);
                StartCoroutine("MoveFrontHovering");
            }
            if (KeyBoardInputSetting._instance.moveBack)
            {
                droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,-10f*tits.transform.forward,Time.deltaTime*10f) ;
                //droneRigi.velocity = -10f*tits.transform.forward;
                //transform.rotation = Quaternion.Euler(-30f,transform.rotation.eulerAngles.y, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-30f,transform.rotation.eulerAngles.y, 0f),Time.deltaTime*10f);
                StartCoroutine("MoveBackHovering");
            }
            if (KeyBoardInputSetting._instance.moveRight)
            {
                droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,10f*tits.transform.right,Time.deltaTime*10f) ;
                //droneRigi.velocity = 10f*tits.transform.right;
                //transform.rotation = Quaternion.Euler(0f,transform.rotation.eulerAngles.y, -30f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f,transform.rotation.eulerAngles.y, -30f),Time.deltaTime*10f);
                StartCoroutine("MoveRightHovering");
            }
            if (KeyBoardInputSetting._instance.moveLeft)
            {
                droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,-10f*tits.transform.right,Time.deltaTime*10f) ;
                //droneRigi.velocity = -10f*tits.transform.right;
                //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 30f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 30f),Time.deltaTime*10f);
                StartCoroutine("MoveLeftHovering");
            }
            if (KeyBoardInputSetting._instance.rotateLeft)
            {
                //transform.Rotate(0,-90,0);
                StartCoroutine(SlowlyRotate(90,0,-1));
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y-90, 0),Time.deltaTime*100f);
                //transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, transform.eulerAngles+new Vector3(0,-90,0), Time.deltaTime * 10f);
            }
            if (KeyBoardInputSetting._instance.rotateRight)
            {
                StartCoroutine(SlowlyRotate(90,0,1));
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y+90, 0),Time.deltaTime*100f);
                //transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, transform.eulerAngles+new Vector3(0,90,0), Time.deltaTime * 10f);
                //transform.Rotate(0,90,0);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (KeyBoardInputSetting._instance.startKeyControl)
        {
            droneRigi.AddForce(Vector3.up* droneRigi.mass*9.8f);
            if (onSky == false)
            {
                droneRigi.AddForce(Vector3.down* droneRigi.mass*9.8f);
            }
        }
    }

    void rotateDroneFake()
    {
        propeller01.transform.Rotate(new Vector3(0, 0, 100 * 100f));
        propeller02.transform.Rotate(new Vector3(0, 0, -100 * 100f));
        propeller03.transform.Rotate(new Vector3(0, 0, 100 * 100f));
        propeller04.transform.Rotate(new Vector3(0, 0, -100 * 100f));
    }

    void backHovering()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0)),Time.deltaTime*10f);
        //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y,0));
        droneRigi.velocity = Vector3.Lerp(droneRigi.velocity,Vector3.zero,Time.deltaTime*10f) ;
        //droneRigi.velocity = Vector3.zero;
        if (Mathf.Abs(droneRigi.velocity.y)<=1f&& Mathf.Abs(droneRigi.velocity.x)<=1f&& Mathf.Abs(droneRigi.velocity.z)<=1f)
        {
            droneRigi.velocity = Vector3.zero;
        }
    }
    
    IEnumerator TakeOffHovering()
    {
        yield return new WaitForSeconds(2.0f);
        backHovering();
        KeyBoardInputSetting._instance.takeOff = false;
    }
    IEnumerator DescentHovering()
    {
        yield return new WaitForSeconds(0.0f);
        backHovering();
        KeyBoardInputSetting._instance.descent = false;
    }
    

    IEnumerator MoveFrontHovering()
    {
        yield return new WaitForSeconds(0.7f);
        backHovering();
        KeyBoardInputSetting._instance.moveFront = false;
    }
    IEnumerator MoveBackHovering()
    {
        yield return new WaitForSeconds(0.7f);
        backHovering();
        KeyBoardInputSetting._instance.moveBack = false;
    }
    IEnumerator MoveRightHovering()
    {
        yield return new WaitForSeconds(0.7f);
        backHovering();
        KeyBoardInputSetting._instance.moveRight = false;
    }
    IEnumerator MoveLeftHovering()
    {
        yield return new WaitForSeconds(0.7f);
        backHovering();
        KeyBoardInputSetting._instance.moveLeft = false;
    }
    IEnumerator SlowlyRotate(int targetRotate,int oriRotate,int deltaRotate)
    {
        while (oriRotate < targetRotate)
        {
            yield return new WaitForSeconds(0.01f);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y+deltaRotate, 0);
            oriRotate += 1;
        }
    }
}
