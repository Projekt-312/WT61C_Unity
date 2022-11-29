using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AccelerationVlocityControl: MonoBehaviour
    {
        public Vector3 accInput = Vector3.zero;
        public Vector3 angInput = Vector3.zero;
        public float acc_g = 9.80665f;
        //private Vector3 lastInput;
        public Vector3 velocityShown;
        public Vector3 rotationShown;

        private Rigidbody rigi;
        private Vector3 lastVelocity;
        private Vector3 nowVelocity;
        //private float timer;

        private void Start()
        {
            //lastInput = accInput;
            rigi = GetComponent<Rigidbody>();
            lastVelocity = rigi.velocity;
            //timer = 0f;
            velocityShown = rigi.velocity;
        }

        private void FixedUpdate()//0.02s
        {
            //速度控制
            velocityShown = rigi.velocity;
            rotationShown = rigi.rotation.eulerAngles;
            //lastInput = accInput;
            nowVelocity = lastVelocity + accInput * acc_g * 0.02f;//v = v0 + at
            lastVelocity = nowVelocity;
            rigi.velocity = nowVelocity;
            
            //角度控制
            rigi.rotation = Quaternion.Euler(angInput);
            
            // timer += Time.fixedDeltaTime;//当加速度没有产生变化时，计时器启动
            // if ((accInput - lastInput).magnitude > 0.01)//input的加速器发生变化（能够微调的传感器几乎一直在变化）
            // {
            //     //print("has changed");
            //     lastInput = accInput;
            //     nowVelocity = lastVelocity + accInput * acc_g * timer;//v = v0 + at
            //     lastVelocity = nowVelocity;
            //     rigi.velocity = nowVelocity;
            //     timer = 0f;//一个速度变化发生，计时器清零
            // }
            
        }
    }
}