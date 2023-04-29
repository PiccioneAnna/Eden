using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Place Object")]
public class PlaceObject : ToolAction
{
    public override bool OnApplyToTileMap(Vector3Int gridPosition, TilemapReadController tilemapReadController, Item item)
    {
        // If there is an object already in the position then return
        if (tilemapReadController.objectsManager.Check(gridPosition))
        {
            return false;
        }

        GameManager.instance.GetComponent<InventoryManager>().inventoryManager.RemoveItem(item);
        tilemapReadController.objectsManager.Place(item, gridPosition);
        return true;
    }
}
