using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, BombManager> bombs = new Dictionary<int, BombManager>();
    public static Dictionary<int, ExplosionManager> explosions = new Dictionary<int, ExplosionManager>();
    public PlayerController playerController;

    [SerializeField]
    private GameObject localPlayerPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private bool gameStarted = false;
    [SerializeField]
    private bool gameRunning;

    public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    public bool GameRunning { get => gameRunning; set => gameRunning = value; }

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

    public void SpawnPlayer(int _id, string _username, Vector3 _position)
    {
        GameObject _player;

        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, Quaternion.identity);
            if (_player.GetComponent<PlayerController>() != null)
            {
                playerController = _player.GetComponent<PlayerController>();
                playerController.Id = _id;
                playerController.ServerPosition = _position;
            }
        }
        else
            _player = Instantiate(playerPrefab, _position, Quaternion.identity);

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnBomb(int _id, Vector3 _position)
    {
        GameObject _bomb = Instantiate(bombPrefab, _position, Quaternion.identity);
        _bomb.GetComponent<BombManager>().Initialize(_id);
        bombs.Add(_id, _bomb.GetComponent<BombManager>());
    }

    public void SpawnExplosion(int _id, Vector3 _position)
    {
        GameObject _explosion = Instantiate(explosionPrefab, _position, Quaternion.identity);
        _explosion.GetComponent<ExplosionManager>().Initialize(_id);
        explosions.Add(_id, _explosion.GetComponent<ExplosionManager>());
    }
}
