using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");

        if (_fromClient != _clientIdCheck)
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient} has assumed the wrong Client ID ({_clientIdCheck}).");

        Server.clients[_fromClient].username = _username;
        //Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        // Read the length of the array that was sent first.
        bool[] _inputs = new bool[_packet.ReadInt()];

        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        if (Server.clients.TryGetValue(_fromClient, out Client client))
        {
            if (client != null && client.player != null)
                client.player.SetInputs(_inputs);
        }

        //Server.clients[_fromClient].player.SetInputs(_inputs);
    }
}
