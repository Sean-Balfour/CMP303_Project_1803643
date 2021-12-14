using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    CLEAR,
    WALL,
    SOLID,
}

public enum TileContaining
{
    NONE,
    BOMB,
    EXPLOSION
}

public class Tile : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private TileType tType;
    [SerializeField]
    private TileContaining containing;

    public BoxCollider2D BoxCollider { get => boxCollider; }

    public TileType TType
    { 
        get => tType; 
        set 
        { 
            tType = value;
            if (spriteRenderer != null)
                spriteRenderer.sprite = sprites[(int)tType];

            if (boxCollider != null)
            {
                if (value == TileType.CLEAR)
                    boxCollider.isTrigger = true;
                else
                    boxCollider.isTrigger = false;
            }

            ServerSend.UpdateTile(this);
        } 
    }
    public TileContaining Containing { get => containing; set => containing = value; }

    public void Initialize()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprites[(int)tType];
        if (tType == TileType.CLEAR)
            boxCollider.isTrigger = true;
        else
            boxCollider.isTrigger = false;
    }
}
