using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Network.Sockets
{
    public class Server : MonoBehaviour
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, SocketClient> Clients = new Dictionary<int, SocketClient>();
        private static TcpListener _tcpListener;

        public void Start()
        {
            StartServer(2, 26950);
        }

        public static void StartServer(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            InitializeServerData();

            Debug.Log("Starting server...");

            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            Debug.Log($"Server started on port {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = _tcpListener.EndAcceptTcpClient(_result);
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}");

            for (int i = 0; i < MaxPlayers; i++)
            {
                if (Clients[i].tcp.socket == null)
                {
                    Debug.Log($"Client{i} is joined.");
                    Clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect. Server is full!");
        }

        private static void InitializeServerData()
        {
            for (int i = 0; i < MaxPlayers; i++)
            {
                Clients.Add(i, new SocketClient(i));
            }
        }
    }
}