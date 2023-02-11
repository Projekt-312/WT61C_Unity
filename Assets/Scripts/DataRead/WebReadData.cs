using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net;
using System.Text;
using System;

public class WebReadData : MonoBehaviour
{
    public Socket clientSocket;
    private static byte[] result = new byte[1024];
    private void Start()
    {
        IPAddress ip = IPAddress.Parse("192.168.118.124");
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(new IPEndPoint(ip, 80));
            Debug.Log("Connect Success");
        }catch(Exception e)
        {
            Debug.Log(e);
            return;
        }
        StartCoroutine("sendData");
    }

    IEnumerator sendData()
    {
        while (true)
        {
            try
            {
                int receiveLength = clientSocket.Receive(result);
                if (receiveLength > 0)
                {
                    Debug.Log(Encoding.ASCII.GetString(result, 0, receiveLength));
                    //print(receiveLength);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                StopCoroutine("sendData");
            }
            //clientSocket.Send(Encoding.ASCII.GetBytes(""));
            yield return new WaitForSeconds(0.025f);
        }
    }
}
