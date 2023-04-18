using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Pick Up")]
public class TilePickupAction : ToolAction
{
    public override bool OnApplyToTileMap(Vector3 gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        tilemapReadController.cropsManager.PickUp(gridPosition);

        return true;
    }
}
