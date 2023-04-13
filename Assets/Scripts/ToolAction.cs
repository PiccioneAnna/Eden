using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 worldPoint)
    {
        Debug.LogWarning("OnApply is not implimented");
        return true;
    }

    public virtual bool OnApplyToTileMap(Vector3Int tilemapPosition, TilemapReadController tilemapReadController)
    {
        Debug.LogWarning("OnApplyToTileMap is not implimented");
        return true;
    }

    public virtual void OnItemUsed(Item usedItem, InventoryManager inventory)
    {

    }
}
