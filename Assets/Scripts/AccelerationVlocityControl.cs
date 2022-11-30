using System;
using UnityEngine;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class AccelerationVlocityControl: MonoBehaviour
    {
        public Vector3 accInput = Vector3.zero;
        public Vector3 angInput = Vector3.zero;
        public float acc_g = 9.80665f;
        public string sensorInput;

        //private Vector3 lastInput;
        public Vector3 velocityShown;
        public Vector3 rotationShown;

        private Rigidbody rigi;
        private Vector3 lastVelocity;
        private Vector3 nowVelocity;
        private Vector3 lastAngleInput;

        private float[] senSorFloats;
        //private float timer;

        private void Start()
        {
            //lastInput = accInput;
            rigi = GetComponent<Rigidbody>();
            lastVelocity = rigi.velocity;
            //timer = 0f;
            velocityShown = rigi.velocity;
            lastAngleInput = Vector3.zero;
        }

        public void Ticked()//0.02s
        {
            //结果展示
            //print("sensorInput: "+sensorInput);
            velocityShown = rigi.velocity;
            rotationShown = rigi.rotation.eulerAngles;
            
            float[] senSorFloats = new float[6];
            senSorFloats = StringSplit(sensorInput);
            accInput = new Vector3(senSorFloats[0], senSorFloats[1], senSorFloats[2]);
            angInput = new Vector3(senSorFloats[3], senSorFloats[4], senSorFloats[5]);
            
            //lastInput = accInput;
            nowVelocity = lastVelocity + accInput * acc_g * 0.02f;//v = v0 + at
            lastVelocity = nowVelocity;
            rigi.velocity = nowVelocity;
            
            //角度控制
            SetX(lastAngleInput,angInput);
            SetY(lastAngleInput,angInput);
            SetZ(lastAngleInput,angInput);
            lastAngleInput = angInput;
            //rigi.rotation = Quaternion.Euler(angInput);

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

        private float[] StringSplit(string inputString)
        {
            var data1 = inputString.Split(' ');
            List<string> splitedList = new List<string>();
            foreach (string s in data1)
            {
                if (s.Trim() != "")
                {
                    splitedList.Add(s);
                }
            }
            
            float[] outputString = new float[6];
            
            outputString[0] = float.Parse(splitedList[1]);
            outputString[1] = float.Parse(splitedList[2]);
            outputString[2] = float.Parse(splitedList[3]);
            outputString[3] = float.Parse(splitedList[9]);
            outputString[4] = float.Parse(splitedList[10]);
            outputString[5] = float.Parse(splitedList[11]);
            //senSorFloats = outputString;
            print("outputstring ="+outputString[0]+outputString[1]+outputString[2]+outputString[3]+outputString[4]+outputString[5]);
            return outputString;
        }
        void SetX(Vector3 oldAngleInput,Vector3 nowAngleInput)
        {
            Quaternion t_adj = Quaternion.AngleAxis(nowAngleInput.x-oldAngleInput.x, Vector3.up);
            Quaternion t_delta = gameObject.transform.localRotation * t_adj;
            gameObject.transform.localRotation = t_delta;
        }
        void SetY(Vector3 oldAngleInput,Vector3 nowAngleInput)
        {
            Quaternion t_adj = Quaternion.AngleAxis(nowAngleInput.y-oldAngleInput.y, -Vector3.right);
            Quaternion t_delta = gameObject.transform.localRotation * t_adj;
            gameObject.transform.localRotation = t_delta;
        }
        void SetZ(Vector3 oldAngleInput,Vector3 nowAngleInput)
        {
            Quaternion t_adj = Quaternion.AngleAxis(nowAngleInput.z-oldAngleInput.z, Vector3.forward);
            Quaternion t_delta = gameObject.transform.localRotation * t_adj;
            gameObject.transform.localRotation = t_delta;
        }
    }
}