using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Till")]
public class TillTile : ToolAction
{
    [SerializeField] List<TileBase> canTill;
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        TileBase tileToTill= tilemapReadController.GetTileBase(gridPosition);

        if (!canTill.Contains(tileToTill))
        {
            return false;
        }

        tilemapReadController.cropsManager.Till(gridPosition);

        return true;
    }
}
