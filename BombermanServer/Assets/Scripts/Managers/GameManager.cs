using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int playersAlive;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }

    public void StartGame()
    {
        playersAlive = 0;

        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                playersAlive++;
                Server.clients[i].SendIntoGame(Server.clients[i].username);
            }
        }

        ServerSend.StartGame();
    }

    private void Update()
    {
        playersAlive = 0;

        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                if (Server.clients[i].player != null)
                {
                    if (Server.clients[i].player.Health > 0)
                    {
                        playersAlive++;
                    }
                }
            }
        }

        if (playersAlive == 1)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        int _winner = 0;

        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                if (Server.clients[i].player != null)
                {
                    if (Server.clients[i].player.Health > 0)
                    {
                        _winner = i;
                    }
                }
            }
        }

        UIManager.instance.EndGame(_winner);
        ServerSend.AnnounceWinner(_winner);
    }
}
