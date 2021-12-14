using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    #region TCP
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
                Server.clients[i].tcp.SendData(_packet);
        }
    }
    #endregion

    #region UDP
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
                Server.clients[i].udp.SendData(_packet);
        }
    }
    #endregion

    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.Username);
            _packet.Write(_player.transform.position);
            
            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.transform.position);

            // Send through UDP since we're sending a lot of these and we can afford to lose some.
            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnBomb(Bomb _bomb)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnBomb))
        {
            _packet.Write(_bomb.Id);
            _packet.Write(_bomb.transform.position);

            SendTCPDataToAll(_packet);
        }
    }

    public static void Explode(Bomb _bomb)
    {
        using (Packet _packet = new Packet((int)ServerPackets.explode))
        {
            _packet.Write(_bomb.Id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnExplosion(Explosion _explosion)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnExplosion))
        {
            _packet.Write(_explosion.Id);
            _packet.Write(_explosion.transform.position);

            SendTCPDataToAll(_packet);
        }
    }

    public static void RemoveExplosion(Explosion _explosion)
    {
        using (Packet _packet = new Packet((int)ServerPackets.removeExplosion))
        {
            _packet.Write(_explosion.Id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHealth))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.Health);

            SendTCPDataToAll(_packet);
        }
    }

    public static void UpdateTile(Tile _tile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateTile))
        {
            _packet.Write(_tile.name);
            _packet.Write((int)_tile.TType);

            SendTCPDataToAll(_packet);
        }
    }

    public static void StartGame()
    {
        using (Packet _packet = new Packet((int)ServerPackets.startGame))
        {
            _packet.Write(true);

            SendTCPDataToAll(_packet);
        }
    }

    public static void AnnounceWinner(int _winner)
    {
        using (Packet _packet = new Packet((int)ServerPackets.announceWinner))
        {
            _packet.Write(_winner);

            SendTCPData(_winner, _packet);
        }

        using (Packet _packet = new Packet((int)ServerPackets.announceLoser))
        {
            _packet.Write(_winner);

            SendTCPDataToAll(_winner, _packet);
        }
    }

    public static void CloseServer()
    {
        using (Packet _packet = new Packet((int)ServerPackets.closeServer))
        {
            _packet.Write(true);

            SendTCPDataToAll(_packet);
        }
    }
}
