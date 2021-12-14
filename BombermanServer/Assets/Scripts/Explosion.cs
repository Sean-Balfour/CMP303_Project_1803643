using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public static Dictionary<int, Explosion> explosions = new Dictionary<int, Explosion>();
    public static int nextId = 1;

    private int id;
    [SerializeField]
    private float timer;
    [SerializeField]
    private Tile tile;

    public int Id { get => id; }
    public Tile Tile { get => tile; set => tile = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        id = nextId;
        nextId++;

        explosions.Add(id, this);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            if (tile != null)
                tile.Containing = TileContaining.NONE;

            ServerSend.RemoveExplosion(this);

            explosions.Remove(id);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(1);
        }
    }
}
