using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RawConnectionManager : MonoBehaviour
{
    private TcpClient socketForServer;
    private NetworkStream networkStream;
    private StreamWriter streamWriter;
    private StreamReader streamReader;
    public PTPHandshake serializer;
    public PTPHeader ptpHeader;
    public bool handshake = false;
    public bool update = false;

    public bool connected;

    private readonly Queue<string> data = new Queue<string>();

    private void Start()
    {
        CreateConnection("10.5.0.44");
    }

    public void Send(string msg)
    {
        streamWriter.WriteLine(msg);
        streamWriter.Flush();
    }

    void FixedUpdate()
    {
        lock (data)
        {
            if (data.Count != 0)
            {
                string s = data.Dequeue();
                //TUTAJ CZYTA
            }
        }
    }

    void OnDestroy()
    {
        if (networkStream != null)
        {
            networkStream.Close();
            streamWriter.Close();
            streamReader.Close();
        }
    }

    public void CreateConnection(string ip)
    {
        try
        {
            socketForServer = new TcpClient(ip, 10);
        }
        catch
        {
            Debug.Log("could not connect to "+ip);
            return;
        }
        networkStream = socketForServer.GetStream();
        streamReader = new StreamReader(networkStream);
        streamWriter = new StreamWriter(networkStream);
        Debug.Log("conncted");
        connected = true;

        Thread readingThread = new Thread(Listeners);
        readingThread.Start();
    }

    private void Listeners()
    {
        const int refreshRate = 500;

        while (connected)
        {
            try
            {
                string theString = streamReader.ReadLine();
                
                lock (data)
                {
                    data.Enqueue(theString);
                }
                if (handshake == false)
                {
                    serializer = JsonUtility.FromJson<PTPHandshake>(theString);
                    handshake = true;
                }
                else
                {
                    ptpHeader = JsonUtility.FromJson<PTPHeader>(theString);
                    update = true;
                }


                Debug.Log("Message recieved by client:" + theString);
            }
            catch
            {
                lock (data)
                {
                    data.Enqueue("Connection error!");
                }
            }
            Thread.Sleep(refreshRate);
        }
    }
}
