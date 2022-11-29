using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DataUI : MonoBehaviour
    {
        private Text data;
        public AccelerationVlocityControl angularControl;

        private void Start()
        {
            data = GetComponent<Text>();
        }

        private void FixedUpdate()
        {
            Vector3 shownVelocity = angularControl.velocityShown;
            Vector3 shownAcceleration = angularControl.angularInput;
            string tempVelocity = shownVelocity.x.ToString("#0.00") + ", " + shownVelocity.y.ToString("#0.00")  + ", " + shownVelocity.z.ToString("#0.00") ;
            string tempAcceleration = shownAcceleration.x.ToString("#0.00") + ", " + shownAcceleration.y.ToString("#0.00")  + ", " + shownAcceleration.z.ToString("#0.00") ;
            data.text = "AngularVelocity = (x,y,z) = (" + tempVelocity + ")\n"+"Velocity = (x,y,z) = ("+ tempAcceleration + ")";
        }
    }
}