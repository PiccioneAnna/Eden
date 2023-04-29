using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Pick Up")]
public class TilePickupAction : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        tilemapReadController.cropsManager.PickUp(gridPosition);

        tilemapReadController.objectsManager.PickUp(gridPosition);

        return true;
    }
}
