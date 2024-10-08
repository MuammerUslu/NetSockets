using UnityEngine;

namespace Network.Sockets
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg}");
            Client.Instance.myId = _myId;
            // ClientSend.WelcomeReceived();
        }
    }
}