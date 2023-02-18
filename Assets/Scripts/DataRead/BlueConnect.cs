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


public class ClientState
{
    public Socket socket;
    public byte[] data = new byte[1024];
}

public class BlueConnect : MonoBehaviour
{
    private string savePath = System.Environment.CurrentDirectory + @"\ExternalEXE";
    public Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

    // 常量
    public float scaleAccel = 0.00478515625f; // 加速度 [-16g~+16g]    9.8*16/32768
    public float scaleQuat = 0.000030517578125f; // 四元数 [-1~+1]         1/32768
    public float scaleAngle = 0.0054931640625f; // 角度   [-180~+180]     180/32768
    public float scaleAngleSpeed = 0.06103515625f; // 角速度 [-2000~+2000]    2000/32768
    public float scaleMag = 0.15106201171875f; // 磁场 [-4950~+4950]   4950/32768
    public float scaleTemperature = 0.01f; // 温度
    public float scaleAirPressure = 0.0002384185791f; // 气压 [-2000~+2000]    2000/8388608
    public float scaleHeight = 0.0010728836f;    // 高度 [-9000~+9000]    9000/8388608

    //运行时间(ms)
    public int timems;
    // a(m/s^2) 无重力
    public float aX, aY, aZ, absv;
    // a(m/s^2) 有重力
    public float AXg, AYg, AZg, ABSVg;
    // w(/s)
    public float GX, GY, GZ, ABSGW;
    // 四元数
    public float QW, QX, QY, QZ;
    // enur angle
    public float angleX, angleY, angleZ;
    // 3-dim position (m)
    public float rx, ry, rz;
    // 导航系加速度((m/s^2))
    public float asX, asY, asZ, asV;

    //public Socket client;
    public Socket server;
    //public Socket server;
    private static byte[] result = new byte[1024];
    // Start is called before the first frame update
    void Start()
    {
        Thread thread1 = new Thread(InitServer);
        thread1.Start();
        Thread thread2 = new Thread(RunExeByProcess);
        thread2.Start();
        //StartCoroutine("RunExeByProcess", "");
        //Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ////绑定IP和端口号
        //EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        ////绑定方法,绑定IP和端口号
        //server.Bind(point);
        ////开始监听,等待用户连接,设置连接数
        //server.Listen(10);
        ////打印提示
        //print("等待客户端连接");
        //client = server.Accept();
        ////等待连接,阻塞方法
        //print("客户端已连接");
        //StartCoroutine("receiveData");
    }
    public void InitServer()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        server.Bind(point);
        server.Listen(10);
        print("等待客户端连接");
        server.BeginAccept(AcceptCallBack, server);
    }

    public void AcceptCallBack(IAsyncResult ar)
    {

        try
        {
            print("客户端接入");
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            ClientState clientState = new ClientState();
            clientState.socket = client;
            //将连接进来的客户端保存起来
            clients.Add(client, clientState);
            //接收此客户端发来的信息
            client.BeginReceive(clientState.data, 0, 1024, 0, ReceiveCallBack, clientState);
            //继续监听新的客户端接入
            server.BeginAccept(AcceptCallBack, server);
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            ClientState state = ar.AsyncState as ClientState;
            Socket client = state.socket;
            int count = client.EndReceive(ar);
            if (count == 0)
            {
                client.Close();
                clients.Remove(client);
                print("客户端关闭");
                return;
            }
            if (count == 218)
            {
                convertByteToData(Encoding.UTF8.GetString(state.data, 0, count));
            }
            
            //string msg = Encoding.UTF8.GetString(state.data, 0, count);
            //print(msg);
            client.BeginReceive(state.data, 0, 1024, 0, ReceiveCallBack, state);
        }
        catch (SocketException e)
        {
            print(e);
        }
    }

    //IEnumerator receiveData()
    //{
    //    while (true)
    //    {
    //        int length = client.Receive(result);

    //        //接收数据data
    //        //string str = Encoding.UTF8.GetString(result);
    //        print(Encoding.UTF8.GetString(result));
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //}


    public void RunExeByProcess()
    {
        //yield return new WaitForSeconds(0.05f);
        //开启新线程
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //调用的exe名称
        process.StartInfo.FileName = "YH.Bluetooth.exe";
        process.StartInfo.WorkingDirectory = savePath;
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.UseShellExecute = true;
        //传递参数
        //process.StartInfo.Arguments = arguments;
        process.Start();

        process.WaitForExit();//停顿，当外部程序退出后才能继续执行

        if (process.ExitCode == 0)//程序退出
        {
            print("执行成功");
            //StopCoroutine("RunExeByProcess");
        }
        return;
    }

    public void convertByteToData(string msg)
    {
        Debug.Log(timems);
        string[] msgs = msg.Split('-');
        int length = msgs.Length;
        if (!msgs[0].Equals("11"))
        {
            return;
        }
        timems = Convert.ToInt32(msgs[6] + msgs[5] + msgs[4] + msgs[3], 16);
        string binaryText = Convert.ToString(Convert.ToInt32(msgs[1], 16), 2);
        if(binaryText[3] == '1') //订阅无重力加速度
        {
            aX = convertHexToSignInt16(msgs[8] + msgs[7]) * scaleAccel;
            aY = convertHexToSignInt16(msgs[10] + msgs[9]) * scaleAccel;
            aZ = convertHexToSignInt16(msgs[12] + msgs[11]) * scaleAccel;
            absv = Mathf.Sqrt(Mathf.Pow(aX, 2f) + Mathf.Pow(aY, 2f) + Mathf.Pow(aZ, 2f));
        }
        if (binaryText[2] == '1') //订阅有重力加速度
        {
            AXg = convertHexToSignInt16(msgs[14] + msgs[13]) * scaleAccel;
            AYg = convertHexToSignInt16(msgs[16] + msgs[15]) * scaleAccel;
            AZg = convertHexToSignInt16(msgs[18] + msgs[17]) * scaleAccel;
            ABSVg = Mathf.Sqrt(Mathf.Pow(AXg, 2f) + Mathf.Pow(AYg, 2f) + Mathf.Pow(AZg, 2f));
        }
        if (binaryText[1] == '1') //订阅角速度
        {
            GX = convertHexToSignInt16(msgs[20] + msgs[19]) * scaleAngleSpeed;
            GY = convertHexToSignInt16(msgs[22] + msgs[21]) * scaleAngleSpeed;
            GZ = convertHexToSignInt16(msgs[24] + msgs[23]) * scaleAngleSpeed;
            ABSGW = Mathf.Sqrt(Mathf.Pow(GX, 2f) + Mathf.Pow(GY, 2f) + Mathf.Pow(GZ, 2f));
        }

        //获得四元数
        QW = convertHexToSignInt16(msgs[40] + msgs[39]) * scaleQuat;
        QX = convertHexToSignInt16(msgs[42] + msgs[41]) * scaleQuat;
        QY = convertHexToSignInt16(msgs[44] + msgs[43]) * scaleQuat;
        QZ = convertHexToSignInt16(msgs[46] + msgs[45]) * scaleQuat;

        //获得欧拉角
        angleX = convertHexToSignInt16(msgs[48] + msgs[47]) * scaleAngle;
        angleY = convertHexToSignInt16(msgs[50] + msgs[49]) * scaleAngle;
        angleZ = convertHexToSignInt16(msgs[52] + msgs[51]) * scaleAngle;

        //获得三维位置
        rx = convertHexToSignInt16(msgs[54] + msgs[53]) / 1000.0f;
        ry = convertHexToSignInt16(msgs[56] + msgs[55]) / 1000.0f;
        rz = convertHexToSignInt16(msgs[58] + msgs[57]) / 1000.0f;

        //获得导航系加速度
        asX = convertHexToSignInt16(msgs[65] + msgs[64]) * scaleAccel;
        asY = convertHexToSignInt16(msgs[67] + msgs[66]) * scaleAccel;
        asZ = convertHexToSignInt16(msgs[69] + msgs[68]) * scaleAccel;
        asV = Mathf.Sqrt(Mathf.Pow(asX, 2f) + Mathf.Pow(asY, 2f) + Mathf.Pow(asZ, 2f));

        //print(aX + " " + aY + " " + aZ);

    }
    public int convertHexToSignInt16(string msg)
    {
        int temp = Convert.ToInt32(msg, 16);
        if(temp >= (1 << 15))
        {
            temp -= (0xFFFF);
        }
        return temp;
    }
}
