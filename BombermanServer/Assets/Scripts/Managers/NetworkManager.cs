using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<GameObject> spawns;

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public void StartServer(int _port)
    {
        Server.Start(4, _port);
    }

    public Player InstantiatePlayer(int id)
    {
        return Instantiate(playerPrefab, spawns[id - 1].transform.position, Quaternion.identity).GetComponent<Player>();
    }
}
