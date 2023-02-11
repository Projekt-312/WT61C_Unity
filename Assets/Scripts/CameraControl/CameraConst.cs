using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraConst : MonoBehaviour
    {
        public GameObject model;

        private void Update()
        {
            transform.localPosition = model.transform.localPosition;
        }
    }
}