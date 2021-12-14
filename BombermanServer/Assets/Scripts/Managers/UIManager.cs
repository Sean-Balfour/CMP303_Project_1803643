using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private GameObject playMenu;
    [SerializeField]
    private Text playersText;
    [SerializeField]
    private InputField portField;
    [SerializeField]
    private GameObject endWindow;
    [SerializeField]
    private Text endText;

    public InputField PortField { get => portField; }

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

    private void Update()
    {
        if (playMenu.activeSelf == true)
        {
            int connected = 0;

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (Server.clients[i].tcp.socket != null)
                    connected++;
            }

            playersText.text = "Players connected: " + connected.ToString() + "/4";
        }
    }

    public void StartServer()
    {
        startMenu.SetActive(false);
        portField.interactable = false;

        NetworkManager.instance.StartServer(int.Parse(portField.text));

        playMenu.SetActive(true);
    }

    public void EndGame(int _winner)
    {
        startMenu.SetActive(false);
        playMenu.SetActive(false);
        endWindow.SetActive(true);

        endText.text = $"Game over!\nCongratulations to {Server.clients[_winner].player.Username}!";
    }

    public void StopServer()
    {
        Server.Stop();

        Application.Quit();
    }

    public void StartGame()
    {
        playMenu.SetActive(false);

        GameManager.instance.StartGame();
    }
}
