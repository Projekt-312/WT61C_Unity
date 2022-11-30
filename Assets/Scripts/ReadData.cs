using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace DefaultNamespace
{
    public class ReadData : MonoBehaviour
    {
        public AccelerationVlocityControl acVcon;

        private bool timeTrigger;
        private void Start()
        {
            //dataset = File.ReadAllText(Application.streamingAssetsPath + "/dataWrite.txt");
            TryReadData(Application.streamingAssetsPath + "/dataWrite.txt",Application.streamingAssetsPath + "/dataWriteAng.txt");
            // string testLine = "today[you are OK!";
            // var linePart = testLine.Split('[');
            // print(linePart[0]);
        }

        // private void FixedUpdate()
        // {
        //     float timer = 0;
        //
        //     timer += Time.deltaTime;
        //     if (timer >= 1f)
        //     {
        //         timer = 0f;
        //         timeTrigger = true;
        //     }
        //     
        //     TryReadData(Application.streamingAssetsPath + "/dataWrite.txt",timeTrigger);
        // }

        public void TryReadData(string path1,string path2)
        {
            var accReader = new StreamReader(path1);
            var aguReader = new StreamReader(path1);
            StartCoroutine(WaitAndRead(accReader,aguReader));
            // while ((line = sr.ReadLine()) != null)
                // {
                //     
                //     //print("line:"+line);
                //     var data = line.Split('[');
                //     print("data:"+data[0]+' '+data[1]+' '+data[2]);
                //     acVcon.angularInput = new Vector3(float.Parse(data[0]),float.Parse(data[1]),float.Parse(data[2]));
                // }

        }

        private IEnumerator WaitAndRead(StreamReader dataFile1,StreamReader dataFile2)
        {
            var line1 = string.Empty;
            while ((line1 = dataFile1.ReadLine()) != null)
            {
                yield return new WaitForSeconds(0.02f);
                var line2 = string.Empty;
                line2 = dataFile2.ReadLine();
                
                var data1 = line1.Split('[');
                var data2 = line2.Split('[');
                
                //print("time: "+Time.time);
                //print( "data:"+data1[0]+' '+data1[1]+' '+data1[2]);
                acVcon.accInput = new Vector3(float.Parse(data1[0]),float.Parse(data1[1]),float.Parse(data1[2]));
                acVcon.angInput = new Vector3(float.Parse(data2[0]),float.Parse(data2[1]),float.Parse(data2[2]));
            }
        }
    }
}