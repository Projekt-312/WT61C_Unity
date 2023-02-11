using UnityEngine;
using System.IO;
using DefaultNamespace;

public class ReadTxt : MonoBehaviour
{
    public string[] data;
    private string path;
    public string data_string;
    public AccelerationVlocityControl accVcon;
    private void Start()
    {
        path = "Z:\\helicopter\\Sensor\\file.txt";
    }

    private void ReadText01()  // 01����
    {
        data = File.ReadAllLines(path);
        //Debug.Log(data[0]);
        data_string = data[0];
        accVcon.sensorInput = data_string;
    }

    
    private void FixedUpdate()
    {
        //ReadText01();
        if (File.Exists(path))
        {
            ReadText01();
            //print("send");
            accVcon.Ticked();
        }
    }
}
