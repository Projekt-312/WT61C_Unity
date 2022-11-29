using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AccelerationVlocityControl: MonoBehaviour
    {
        public Vector3 angularInput = Vector3.zero;
        private Vector3 lastInput;
        public Vector3 velocityShown;

        private Rigidbody rigi;
        private Vector3 lastVelocity;
        private Vector3 nowVelocity;
        private float timer;

        private void Start()
        {
            lastInput = angularInput;
            rigi = GetComponent<Rigidbody>();
            lastVelocity = rigi.velocity;
            timer = 0f;
            velocityShown = rigi.velocity;
        }

        private void FixedUpdate()//0.02s
        {
            velocityShown = rigi.velocity;
            
            //print(angularInput - lastInput);
            //print("timer:"+timer);
            timer += Time.fixedDeltaTime;//当加速度没有产生变化时，计时器启动
            if ((angularInput - lastInput).magnitude > 0.01)//input的加速器发生变化（能够微调的传感器几乎一直在变化）
            {
                //print("has changed");
                lastInput = angularInput;
                nowVelocity = lastVelocity + angularInput * timer;//v = v0 + at
                lastVelocity = nowVelocity;
                rigi.velocity = nowVelocity;
                timer = 0f;//一个速度变化发生，计时器清零
            }

            //rigi.velocity = new Vector3(1, 3, 4);
        }
    }
}