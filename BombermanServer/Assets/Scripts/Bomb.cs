using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Dictionary<int, Bomb> bombs = new Dictionary<int, Bomb>();
    public static int nextId = 1;

    private int id;
    [SerializeField]
    private int owner;
    [SerializeField]
    private float timer;
    [SerializeField]
    private Tile tile;

    public int Id { get => id; }
    public int Owner { get => owner; set => owner = value; }
    public Tile Tile { get => tile; set => tile = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize()
    {
        id = nextId;
        nextId++;

        bombs.Add(id, this);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 3f)
        {
            tile.Containing = TileContaining.EXPLOSION;

            ServerSend.Explode(this);

            if (Server.clients[owner] != null)
                Server.clients[owner].player.ActiveBombs--;

            GridManager.instance.SpawnExplosion(transform.position, owner);

            bombs.Remove(id);
            Destroy(gameObject);
        }
    }
}
