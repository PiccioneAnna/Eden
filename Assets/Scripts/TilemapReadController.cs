using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapReadController : MonoBehaviour
{
    public static TilemapReadController instance;

    [SerializeField] Tilemap tilemap;
    [SerializeField] public CropsManager cropsManager;
    public PlaceableObjectsReferenceManager objectsManager;
    Vector3 worldPosition;

    public Player player;

    private void Awake()
    {
        instance = this;
    }

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        if (tilemap == null)
        {
            tilemap = UnityEngine.GameObject.Find("BaseTilemap").GetComponent<Tilemap>();
        }

        if(tilemap == null) { return Vector3Int.zero; }

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
        gridPosition = new Vector3Int(gridPosition.x, gridPosition.y, 0);
        return gridPosition;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        if (tilemap == null)
        {
            tilemap = UnityEngine.GameObject.Find("BaseTilemap").GetComponent<Tilemap>();
        }

        if (tilemap == null) { return null; }

        gridPosition = tilemap.WorldToCell(new Vector3(worldPosition.x, worldPosition.y, 0));
        TileBase tile = tilemap.GetTile(gridPosition);
        Debug.Log(tile);

        return tile;
    }
}
