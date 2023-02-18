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

    // ����
    public float scaleAccel = 0.00478515625f; // ���ٶ� [-16g~+16g]    9.8*16/32768
    public float scaleQuat = 0.000030517578125f; // ��Ԫ�� [-1~+1]         1/32768
    public float scaleAngle = 0.0054931640625f; // �Ƕ�   [-180~+180]     180/32768
    public float scaleAngleSpeed = 0.06103515625f; // ���ٶ� [-2000~+2000]    2000/32768
    public float scaleMag = 0.15106201171875f; // �ų� [-4950~+4950]   4950/32768
    public float scaleTemperature = 0.01f; // �¶�
    public float scaleAirPressure = 0.0002384185791f; // ��ѹ [-2000~+2000]    2000/8388608
    public float scaleHeight = 0.0010728836f;    // �߶� [-9000~+9000]    9000/8388608

    //����ʱ��(ms)
    public int timems;
    // a(m/s^2) ������
    public float aX, aY, aZ, absv;
    // a(m/s^2) ������
    public float AXg, AYg, AZg, ABSVg;
    // w(/s)
    public float GX, GY, GZ, ABSGW;
    // ��Ԫ��
    public float QW, QX, QY, QZ;
    // enur angle
    public float angleX, angleY, angleZ;
    // 3-dim position (m)
    public float rx, ry, rz;
    // ����ϵ���ٶ�((m/s^2))
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
        ////��IP�Ͷ˿ں�
        //EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        ////�󶨷���,��IP�Ͷ˿ں�
        //server.Bind(point);
        ////��ʼ����,�ȴ��û�����,����������
        //server.Listen(10);
        ////��ӡ��ʾ
        //print("�ȴ��ͻ�������");
        //client = server.Accept();
        ////�ȴ�����,��������
        //print("�ͻ���������");
        //StartCoroutine("receiveData");
    }
    public void InitServer()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        server.Bind(point);
        server.Listen(10);
        print("�ȴ��ͻ�������");
        server.BeginAccept(AcceptCallBack, server);
    }

    public void AcceptCallBack(IAsyncResult ar)
    {

        try
        {
            print("�ͻ��˽���");
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            ClientState clientState = new ClientState();
            clientState.socket = client;
            //�����ӽ����Ŀͻ��˱�������
            clients.Add(client, clientState);
            //���մ˿ͻ��˷�������Ϣ
            client.BeginReceive(clientState.data, 0, 1024, 0, ReceiveCallBack, clientState);
            //���������µĿͻ��˽���
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
                print("�ͻ��˹ر�");
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

    //        //��������data
    //        //string str = Encoding.UTF8.GetString(result);
    //        print(Encoding.UTF8.GetString(result));
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //}


    public void RunExeByProcess()
    {
        //yield return new WaitForSeconds(0.05f);
        //�������߳�
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //���õ�exe����
        process.StartInfo.FileName = "YH.Bluetooth.exe";
        process.StartInfo.WorkingDirectory = savePath;
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.UseShellExecute = true;
        //���ݲ���
        //process.StartInfo.Arguments = arguments;
        process.Start();

        process.WaitForExit();//ͣ�٣����ⲿ�����˳�����ܼ���ִ��

        if (process.ExitCode == 0)//�����˳�
        {
            print("ִ�гɹ�");
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
        if(binaryText[3] == '1') //�������������ٶ�
        {
            aX = convertHexToSignInt16(msgs[8] + msgs[7]) * scaleAccel;
            aY = convertHexToSignInt16(msgs[10] + msgs[9]) * scaleAccel;
            aZ = convertHexToSignInt16(msgs[12] + msgs[11]) * scaleAccel;
            absv = Mathf.Sqrt(Mathf.Pow(aX, 2f) + Mathf.Pow(aY, 2f) + Mathf.Pow(aZ, 2f));
        }
        if (binaryText[2] == '1') //�������������ٶ�
        {
            AXg = convertHexToSignInt16(msgs[14] + msgs[13]) * scaleAccel;
            AYg = convertHexToSignInt16(msgs[16] + msgs[15]) * scaleAccel;
            AZg = convertHexToSignInt16(msgs[18] + msgs[17]) * scaleAccel;
            ABSVg = Mathf.Sqrt(Mathf.Pow(AXg, 2f) + Mathf.Pow(AYg, 2f) + Mathf.Pow(AZg, 2f));
        }
        if (binaryText[1] == '1') //���Ľ��ٶ�
        {
            GX = convertHexToSignInt16(msgs[20] + msgs[19]) * scaleAngleSpeed;
            GY = convertHexToSignInt16(msgs[22] + msgs[21]) * scaleAngleSpeed;
            GZ = convertHexToSignInt16(msgs[24] + msgs[23]) * scaleAngleSpeed;
            ABSGW = Mathf.Sqrt(Mathf.Pow(GX, 2f) + Mathf.Pow(GY, 2f) + Mathf.Pow(GZ, 2f));
        }

        //�����Ԫ��
        QW = convertHexToSignInt16(msgs[40] + msgs[39]) * scaleQuat;
        QX = convertHexToSignInt16(msgs[42] + msgs[41]) * scaleQuat;
        QY = convertHexToSignInt16(msgs[44] + msgs[43]) * scaleQuat;
        QZ = convertHexToSignInt16(msgs[46] + msgs[45]) * scaleQuat;

        //���ŷ����
        angleX = convertHexToSignInt16(msgs[48] + msgs[47]) * scaleAngle;
        angleY = convertHexToSignInt16(msgs[50] + msgs[49]) * scaleAngle;
        angleZ = convertHexToSignInt16(msgs[52] + msgs[51]) * scaleAngle;

        //�����άλ��
        rx = convertHexToSignInt16(msgs[54] + msgs[53]) / 1000.0f;
        ry = convertHexToSignInt16(msgs[56] + msgs[55]) / 1000.0f;
        rz = convertHexToSignInt16(msgs[58] + msgs[57]) / 1000.0f;

        //��õ���ϵ���ٶ�
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
