using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");

        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();

        GameManager.instance.SpawnPlayer(_id, _username, _position);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (Client.instance.myId == _id)
        {
            // UDP can be faster than TCP, so check if player exists before trying to move them.
            if (GameManager.instance.playerController != null)
            {
                GameManager.instance.playerController.ServerPosition = _position;
            }
        }
        else
        {
            // UDP can be faster than TCP, so check if player exists before trying to move them.
            if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
            {
                _player.transform.position = _position;
            }
        }
    }

    public static void SpawnBomb(Packet _packet)
    {
        int _bombId = _packet.ReadInt();
        Vector3 _bombPos = _packet.ReadVector3();

        GameManager.instance.SpawnBomb(_bombId, _bombPos);
    }


    public static void Explode(Packet _packet)
    {
        int _bombId = _packet.ReadInt();

        GameManager.bombs[_bombId].Explode();
    }

    public static void SpawnExplosion(Packet _packet)
    {
        int _explosionId = _packet.ReadInt();
        Vector3 _explosionPos = _packet.ReadVector3();

        GameManager.instance.SpawnExplosion(_explosionId, _explosionPos);
    }

    public static void RemoveExplosion(Packet _packet)
    {
        int _explosionId = _packet.ReadInt();

        GameManager.explosions[_explosionId].Remove();
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _health = _packet.ReadInt();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void UpdateTile(Packet _packet)
    {
        string _name = _packet.ReadString();
        int _tileType = _packet.ReadInt();

        GameObject tile = GameObject.Find(_name);

        if (tile)
        {
            tile.GetComponent<Tile>().TType = (TileType)_tileType;
        }
    }

    public static void StartGame(Packet _packet)
    {
        bool _started = _packet.ReadBool();

        GameManager.instance.GameStarted = _started;
        GameManager.instance.GameRunning = _started;
    }

    public static void AnnounceWinner(Packet _packet)
    {
        GameManager.instance.GameRunning = false;

        int _winnerId = _packet.ReadInt();

        UIManager.instance.ShowWinner(_winnerId);
    }

    public static void AnnounceLoser(Packet _packet)
    {
        GameManager.instance.GameRunning = false;

        int _winnerId = _packet.ReadInt();

        UIManager.instance.ShowLoser(_winnerId);
    }

    public static void CloseServer(Packet _packet)
    {
        bool close = _packet.ReadBool();

        Application.Quit();
    }
}
