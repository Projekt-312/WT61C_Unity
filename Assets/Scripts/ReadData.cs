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
            TryReadData(Application.streamingAssetsPath + "/dataWrite.txt");
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

        public void TryReadData(string path)
        {
            var sr = new StreamReader(path);
            StartCoroutine(WaitAndRead(sr));
                // while ((line = sr.ReadLine()) != null)
                // {
                //     
                //     //print("line:"+line);
                //     var data = line.Split('[');
                //     print("data:"+data[0]+' '+data[1]+' '+data[2]);
                //     acVcon.angularInput = new Vector3(float.Parse(data[0]),float.Parse(data[1]),float.Parse(data[2]));
                // }

        }

        private IEnumerator WaitAndRead(StreamReader dataFile)
        {
            var line = string.Empty;
            while ((line = dataFile.ReadLine()) != null)
            {
                yield return new WaitForSeconds(0.02f);
                var data = line.Split('[');
                print("time: "+Time.time);
                print( "data:"+data[0]+' '+data[1]+' '+data[2]);
                acVcon.angularInput = new Vector3(float.Parse(data[0]),float.Parse(data[1]),float.Parse(data[2]));
            }
        }
    }
}