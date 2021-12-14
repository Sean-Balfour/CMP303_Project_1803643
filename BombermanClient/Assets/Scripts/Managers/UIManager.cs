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
    private InputField usernameField;
    [SerializeField]
    private InputField ipField;
    [SerializeField]
    private InputField portField;
    [SerializeField]
    private List<GameObject> playerPanels;
    [SerializeField]
    private List<Text> playerNames;
    [SerializeField]
    private List<Text> playerHealths;
    [SerializeField]
    private GameObject winnerWindow;
    [SerializeField]
    private GameObject loserWindow;
    [SerializeField]
    private Text loserText;

    public InputField UsernameField { get => usernameField; }

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
        if (GameManager.instance.GameStarted && GameManager.instance.GameRunning)
        {
            for (int i = 1; i <= GameManager.players.Count; i++)
            {
                playerNames[i - 1].text = GameManager.players[i].Username;
                playerHealths[i - 1].text = GameManager.players[i].Health.ToString() + "/3";

                if (GameManager.players[i].Health <= 0)
                    playerPanels[i - 1].SetActive(false);
                else
                    playerPanels[i - 1].SetActive(true);
            }
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;

        Client.instance.ConnectToServer(ipField.text, portField.text);
    }

    public void ShowWinner(int _winner)
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            playerPanels[i].SetActive(false);
        }
        winnerWindow.SetActive(true);
    }

    public void ShowLoser(int _winner)
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            playerPanels[i].SetActive(false);
        }
        loserWindow.SetActive(true);
        loserText.text = $"Better luck next time!\n{GameManager.players[_winner].Username} is the winner!";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
