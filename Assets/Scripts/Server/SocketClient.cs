using System;
using System.Net.Sockets;
using UnityEngine;

namespace Network.Sockets
{
    public class SocketClient
    {
        public static int dataBufferSize = 4096;

        public int id = 0;

        public TCP tcp;
        // Start is called before the first frame update

        public SocketClient(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(_clientId);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);


                ServerSend.Welcome(id, "Welcome to the server.");
            }
            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); // Send data to appropriate client
                        Debug.Log($"Sending Data Legth:{_packet.Length()}");

                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int _byteLength = stream.EndRead(result);
                    if (_byteLength <= 0)
                    {
                        //TODO disconnect
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving TCP data: {e}");
                    //TODO disconnect
                }
            }
        }
    }
}