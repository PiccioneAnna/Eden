using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Hoe")]
public class HoeTile : ToolAction
{
    [SerializeField] List<TileBase> canHoe;
    public override bool OnApplyToTileMap(Vector3 gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        TileBase tileToPlow = tilemapReadController.GetTileBase(gridPosition);

        if (!canHoe.Contains(tileToPlow))
        {
            return false;
        }

        tilemapReadController.cropsManager.Plow(gridPosition);

        return true;
    }
}
