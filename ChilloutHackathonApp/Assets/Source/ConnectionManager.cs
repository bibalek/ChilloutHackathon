using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{

    public Text statusText;
    public InputField ipField;
    public InputField messageField;
    public Text lastMsg;
    
    public CanvasGroup ipCanvas;
    public CanvasGroup sendCanvas;
    public CanvasGroup receiveCanvas;

    private TcpClient socketForServer;
    private NetworkStream networkStream;
    private StreamWriter streamWriter;
    private StreamReader streamReader;

    public bool connected;

    private readonly Queue<string> data = new Queue<string>();

    #region Button Callbacks

    public void OnButton()
    {
        CreateConnection(ipField.text);
    }

    public void Send()
    {
        if (connected)
        {
            streamWriter.WriteLine(messageField.text);
            streamWriter.Flush();
        }
        else
        {
            statusText.text += "not connected yet!";
        }
    }

    #endregion

    void Awake()
    {
        SetAsInteractable(ipCanvas, true);
        SetAsInteractable(sendCanvas, false);
        SetAsInteractable(receiveCanvas, false);
    }

    void FixedUpdate()
    {
        lock (data)
        {
            if (data.Count != 0)
            {
                string s = data.Dequeue();
                lastMsg.text = s;
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
            statusText.text = string.Format("Failed to connect to server at {0}:999", ip);
            return;
        }
        networkStream = socketForServer.GetStream();
        streamReader = new StreamReader(networkStream);
        streamWriter = new StreamWriter(networkStream);
        statusText.text = "CONNECTED!";
        connected = true;

        SetAsInteractable(ipCanvas, false);
        SetAsInteractable(sendCanvas, true);
        SetAsInteractable(receiveCanvas, true);

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
        SetAsInteractable(ipCanvas, false);
        SetAsInteractable(sendCanvas, false);
        SetAsInteractable(receiveCanvas, false);
    }

    private void SetAsInteractable(CanvasGroup canvasGroup, bool value)
    {
        lock (canvasGroup)
        {
            Selectable[] selectables = canvasGroup.GetComponentsInChildren<Selectable>();
            foreach (Selectable s in selectables)
            {
                s.interactable = value;
            }
            canvasGroup.alpha = value ? 1 : 0.6f;
        }
    }
}
