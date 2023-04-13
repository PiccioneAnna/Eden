using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Seed Tile")]
public class SeedTile : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapReadController)
    {
        if (!tilemapReadController.cropsManager.Check(gridPosition)) { return false; }

        tilemapReadController.cropsManager.Seed(gridPosition);

        return true;
    }

    public override void OnItemUsed(Item usedItem, InventoryManager inventory)
    {
        inventory.RemoveItem(usedItem);
    }
}
