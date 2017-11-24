using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Program
{
    private static readonly TcpListener tcpListener = new TcpListener(10);
    private static readonly List<StreamWriter> writers = new List<StreamWriter>();

    private static void BroadcastMessage(string msg)
    {
        foreach (var writer in writers)
        {
            writer.WriteLine(msg);
            writer.Flush();
        }
    }

    private static void ListenerThread()
    {
        Socket socketForClient = tcpListener.AcceptSocket();
        if (socketForClient.Connected)
        {
            Console.WriteLine("Client:" + socketForClient.RemoteEndPoint + " now connected to server.");
            NetworkStream networkStream = new NetworkStream(socketForClient);

            //add writer for this client to clients pool
            writers.Add(new StreamWriter(networkStream));

            StreamReader streamReader = new StreamReader(networkStream);
            while (true)
            {
                try
                {
                    string theString = streamReader.ReadLine();
                    Console.WriteLine("Message received by client:" + theString);
                    BroadcastMessage(theString);
                }
                catch
                {
                    Console.Write("connection broken");
                    return;
                }
            }
        }
        socketForClient.Close();
        Console.WriteLine("Press any key to exit from server program");
        Console.ReadKey();
    }

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public static void Main()
    {
        tcpListener.Start();

        Console.WriteLine("This is Server program,");
        Console.WriteLine("IP: " + GetLocalIPAddress());
        Console.WriteLine("How many clients are going to connect to this server?:");
        int numberOfClientsYouNeedToConnect = int.Parse(Console.ReadLine());
        for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
        {
            Thread newThread = new Thread(ListenerThread);
            newThread.Start();
        }
    }
}
