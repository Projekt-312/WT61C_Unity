using UnityEngine;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class testRotate: MonoBehaviour
    {
        private string splitString;

        private void Start()
        {
            splitString = "a(g):     1.631      0.845     -0.841 w(deg/s):   -72.510   -221.130   -238.708 Angle(deg):    89.533    -46.143   -127.381";
            string[] splited =  splitString.Split(' ');
            List<string> splitedList = new List<string>();
            foreach (string s in splited)
            {
                if (s.Trim() != "")
                {
                    splitedList.Add(s);
                }
            }
            //print(splitedList.Count);
            for (int i = 0; i < splitedList.Count; i++)
            {
                //print(i+": "+splitedList[i]);
            }
        }
    }
}