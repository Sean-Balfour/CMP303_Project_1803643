using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private int gridSize = 15;

    [SerializeField]
    private List<Tile> tiles;

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
        CreateGrid();
    }

    public void SpawnExplosion(Vector2 _pos, int _owner)
    {
        Tile currentTile = GetTile(_pos);

        Explosion _explosion = Instantiate(explosionPrefab, _pos, Quaternion.identity).GetComponent<Explosion>();
        _explosion.Initialize();
        if (currentTile != null)
        {
            _explosion.Tile = currentTile;
        }

        ServerSend.SpawnExplosion(_explosion);

        StartCoroutine(ContinueExplosion(_owner, _pos));
    }

    private IEnumerator ContinueExplosion(int _owner, Vector2 _startPos)
    {
        bool[] stop = new bool[]
        {
            false, false, false, false
        };

        for (int i = 1; i <= Server.clients[_owner].player.BombRange; i++)
        {
            yield return new WaitForSeconds(0.1f);

            // North
            Tile northTile = GetTile(new Vector2(_startPos.x, _startPos.y + i));
            if (northTile != null)
            {
                if (stop[0] == false)
                {
                    if (northTile.TType == TileType.WALL)
                    {
                        northTile.TType = TileType.CLEAR;
                        stop[0] = true;
                    }
                    else if (northTile.TType == TileType.SOLID)
                    {
                        stop[0] = true;
                    }

                    Explosion _explosion = Instantiate(explosionPrefab, new Vector2(_startPos.x, _startPos.y + i), Quaternion.identity).GetComponent<Explosion>();
                    _explosion.Initialize();
                    _explosion.Tile = northTile;

                    ServerSend.SpawnExplosion(_explosion);
                }
            }
            // East
            Tile eastTile = GetTile(new Vector2(_startPos.x + i, _startPos.y));
            if (eastTile != null)
            {
                if (stop[1] == false)
                {
                    if (eastTile.TType == TileType.WALL)
                    {
                        eastTile.TType = TileType.CLEAR;
                        stop[1] = true;
                    }
                    else if (eastTile.TType == TileType.SOLID)
                    {
                        stop[1] = true;
                    }

                    Explosion _explosion = Instantiate(explosionPrefab, new Vector2(_startPos.x + i, _startPos.y), Quaternion.identity).GetComponent<Explosion>();
                    _explosion.Initialize();
                    _explosion.Tile = eastTile;

                    ServerSend.SpawnExplosion(_explosion);
                }
            }

            // South
            Tile southTile = GetTile(new Vector2(_startPos.x, _startPos.y - i));
            if (southTile != null)
            {
                if (stop[2] == false)
                {
                    if (southTile.TType == TileType.WALL)
                    {
                        southTile.TType = TileType.CLEAR;
                        stop[2] = true;
                    }
                    else if (southTile.TType == TileType.SOLID)
                    {
                        stop[2] = true;
                    }

                    Explosion _explosion = Instantiate(explosionPrefab, new Vector2(_startPos.x, _startPos.y - i), Quaternion.identity).GetComponent<Explosion>();
                    _explosion.Initialize();
                    _explosion.Tile = southTile;

                    ServerSend.SpawnExplosion(_explosion);
                }
            }

            // West
            Tile westTile = GetTile(new Vector2(_startPos.x - i, _startPos.y));
            if (westTile != null)
            {
                if (stop[3] == false)
                {
                    if (westTile.TType == TileType.WALL)
                    {
                        westTile.TType = TileType.CLEAR;
                        stop[3] = true;
                    }
                    else if (westTile.TType == TileType.SOLID)
                    {
                        stop[3] = true;
                    }

                    Explosion _explosion = Instantiate(explosionPrefab, new Vector2(_startPos.x - i, _startPos.y), Quaternion.identity).GetComponent<Explosion>();
                    _explosion.Initialize();
                    _explosion.Tile = westTile;

                    ServerSend.SpawnExplosion(_explosion);
                }
            }
        }
    }

    private bool IsSpawnTile(int i, int j)
    {
        if ((i == 0 && j == 0) || (i == gridSize - 1 && j == gridSize - 1) || (i == gridSize - 1 && j == 0) || (j == gridSize - 1 && i == 0))
            return true;
        else if ((i == 1 && j == 0) || (j == 1 && i == 0))
            return true;
        else if ((i == (gridSize - 2) && j == 0) || (j == 1 && i == (gridSize - 1)))
            return true;
        else if ((i == 0 && j == (gridSize - 2)) || (j == (gridSize - 1) && i == 1))
            return true;
        else if ((i == (gridSize - 1) && j == (gridSize - 2)) || (j == (gridSize - 1) && i == (gridSize - 2)))
            return true;

        return false;
    }

    public bool PlaceBomb(Vector2 _pos, int _owner)
    {
        Tile currentTile = GetTile(_pos);

        if (currentTile != null && currentTile.Containing == TileContaining.NONE)
        {
            currentTile.Containing = TileContaining.BOMB;

            Bomb _bomb = Instantiate(bombPrefab, currentTile.transform.position, Quaternion.identity).GetComponent<Bomb>();
            _bomb.Owner = _owner;
            _bomb.Tile = currentTile;
            _bomb.Initialize();

            ServerSend.SpawnBomb(_bomb);

            return true;
        }
        else
            Debug.Log($"Could not find tile at {_pos}");

        return false;
    }

    private Tile GetTile(Vector2 _pos)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
             if (tiles[i].BoxCollider.bounds.Contains(_pos))
                return tiles[i];
        }

        return null;
    }

    public void CreateGrid()
    {
        tiles = new List<Tile>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Tile tempTile = Instantiate(tilePrefab, new Vector2(j, i), Quaternion.identity).GetComponent<Tile>();

                if (IsSpawnTile(i, j))
                {
                    tempTile.TType = TileType.CLEAR;
                }
                else if ((i + 1) % 2 == 0 && (j + 1) % 2 == 0)
                {
                    tempTile.TType = TileType.SOLID;
                }
                else
                    tempTile.TType = TileType.WALL;

                tempTile.Initialize();
                tempTile.transform.parent = transform;
                tempTile.name = j.ToString() + "x" + i.ToString();

                tiles.Add(tempTile);
            }
        }
    }
}
