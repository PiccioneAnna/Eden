using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapReadController : MonoBehaviour
{
    public static TilemapReadController instance;

    [SerializeField] Tilemap tilemap;
    [SerializeField] public CropsManager cropsManager;
    Vector3 worldPosition;

    public Player player;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 GetGridPosition(Vector2 position, bool mousePosition)
    {

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3 gridPosition = tilemap.WorldToLocal(worldPosition);
        return gridPosition;
    }

    public TileBase GetTileBase(Vector3 gridPosition)
    {
        TileBase tile = tilemap.GetTile(new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0));
        Debug.Log(tile);

        return tile;
    }
}
