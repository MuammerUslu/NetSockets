using UnityEditor;
using UnityEngine;

namespace Network.Sockets
{
    [CustomEditor(typeof(Client))]
    public class ClientEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Client clientScript = (Client)target;

            if (GUILayout.Button("Connect"))
            {
                clientScript.ConnectToServer();
            }
        }
    }  
}

