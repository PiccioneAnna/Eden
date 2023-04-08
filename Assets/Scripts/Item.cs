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
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public GameObject obj;
    public Sprite image;
}

public enum ItemType
{
    BuildingBlock,
    Material,
    Tool
}

public enum ActionType
{
    Dig,
    Mine,
    Chop,
    Hoe,
    None
}
