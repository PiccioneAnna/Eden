using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public TileBase tile;
    public string itemName;
    public bool usesGrid = false;
    public Vector2Int range = new Vector2Int(5, 4);
    public Crop crop;
    public ItemType itemType;

    [Header("Only UI")]
    public bool stackable = true;
    public bool shopItem = false;
    public int levelRequirement = 1;
    public CraftRecipe recipe;

    [Header("Both")]
    public UnityEngine.GameObject obj;
    public GameObject itemPrefab;
    public Sprite image;
    public bool iconHighlight;
    public bool isWeapon;
    public int damage = 5;

    public ToolAction onAction;
    public ToolAction onTileMapAction;
    public ToolAction onItemUsed;

    public enum ItemType
    {
        Crop,
        Material,
        Tool,
        PlaceableObject,
        Consumable
    }
}
