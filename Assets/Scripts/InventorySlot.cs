using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Item item;
    public InventoryItem inventoryItem;
    public Image image;
    public Color selectedColor, notSelectedColor;
    public Crafting crafting;

    public Item ItemInSlot { get{ return item; } set { item = value; } }

    private void Awake()
    {
        Deselect();
        crafting = GameManager.instance.gameObject.GetComponent<CraftingManager>().crafting;
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    // Drag and Drop
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

    public void Clear()
    {
        inventoryItem = GetComponentInChildren<InventoryItem>();
        inventoryItem = null;
    }

    public void CraftButton()
    {
        if (ItemInSlot != null)
        {
            if (ItemInSlot.recipe != null)
            {
                crafting.Craft(ItemInSlot.recipe);
            }
        }
    }
}
