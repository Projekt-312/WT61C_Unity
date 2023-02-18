using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System;

public class ControlTello : MonoBehaviour
{
    public Socket server;
    EndPoint localaddr, telloaddr;
    Thread thread;
    WaitForSeconds time0 = new WaitForSeconds(0.005f);
    WaitForSeconds time1 = new WaitForSeconds(0f);
    byte[] result = new byte[1024];

    // Start is called before the first frame update
    void Start()
    {
        //thread = new Thread(InitServer);
        //thread.Start();
        print("\r\n\r\nTello Unity Demo.\r\n");
        print("Tello: command takeoff land flip forward back left right \r\n       up down cw ccw speed speed?\r\n");
        print("end -- quit demo.\r\n");
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        localaddr = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 9000);
        telloaddr = new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889);
        server.Bind(localaddr);
        print("等待客户端连接");
        StartCoroutine("sendCommands");
        thread = new Thread(receiveData);
    }

    // Update is called once per frame
    IEnumerator sendCommands()
    {
        while (true)
        {
            bool flag = false;
            if (Input.GetKeyDown(KeyCode.C))
            {
                server.SendTo(Encoding.UTF8.GetBytes("command"), telloaddr);
                print("command");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                print("end");
                thread.Abort();

            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                server.SendTo(Encoding.UTF8.GetBytes("takeoff"), telloaddr);
                print("takeoff");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                server.SendTo(Encoding.UTF8.GetBytes("land"), telloaddr);
                print("land");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                server.SendTo(Encoding.UTF8.GetBytes("forward 20"), telloaddr);
                print("forward");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                server.SendTo(Encoding.UTF8.GetBytes("back 20"), telloaddr);
                print("back");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                server.SendTo(Encoding.UTF8.GetBytes("left 20"), telloaddr);
                print("left");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                server.SendTo(Encoding.UTF8.GetBytes("right 20"), telloaddr);
                print("right");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                server.SendTo(Encoding.UTF8.GetBytes("up 20"), telloaddr);
                print("up");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                server.SendTo(Encoding.UTF8.GetBytes("down 20"), telloaddr);
                print("down");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                server.SendTo(Encoding.UTF8.GetBytes("emergency"), telloaddr);
                print("emergency");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                server.SendTo(Encoding.UTF8.GetBytes("ccw 90"), telloaddr);
                print("ccw");
                flag = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                server.SendTo(Encoding.UTF8.GetBytes("cw 90"), telloaddr);
                print("cw");
                flag = true;
            }
            if (flag)
            {
                yield return time0;
            }
            else
            {
                yield return time1;
            }
        }
    }

    public void receiveData()
    {
        int count = 0;
        while (true)
        {
            count = server.Receive(result);
            print("recv: " + count);
            if (count > 0)
                print(Encoding.UTF8.GetString(result));
        }
    }
}
