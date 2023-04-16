using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public TileBase tile;
    public string itemName;
    public ItemType type;
    public ActionType actionType;
    public bool usesGrid = false;
    public Vector2Int range = new Vector2Int(5, 4);
    public Crop crop;

    [Header("Only UI")]
    public bool stackable = true;
    public ItemRecipe recipe;

    [Header("Both")]
    public UnityEngine.GameObject obj;
    public Sprite image;

    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;
}

public enum ItemType
{
    BuildingBlock,
    Material,
    Seed,
    Tool
}

public enum ActionType
{
    Dig,
    Mine,
    Chop,
    Hoe,
    Seed,
    None
}
