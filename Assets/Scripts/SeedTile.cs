using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Seed Tile")]
public class SeedTile : ToolAction
{
    public override bool OnApplyToTileMap(Vector3 gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        if (!tilemapReadController.cropsManager.Check(gridPosition)) { return false; }

        tilemapReadController.cropsManager.Seed(gridPosition, item.crop);

        return true;
    }

    public override void OnItemUsed(Item usedItem, InventoryManager inventory)
    {
        inventory.RemoveItem(usedItem);
    }
}
