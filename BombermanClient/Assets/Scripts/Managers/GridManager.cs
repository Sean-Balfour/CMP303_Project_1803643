using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private int gridSize = 15;

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

    public void CreateGrid()
    {
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
            }
        }
    }
}
