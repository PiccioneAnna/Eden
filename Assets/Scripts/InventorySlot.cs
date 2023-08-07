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
    public bool craftingSlot = false;

    public Item ItemInSlot { get{ return item; } set { item = value; } }

    private void Awake()
    {
        Deselect();
        crafting = GameManager.instance.GetComponent<CraftingManager>().crafting;
        if(craftingSlot == false)
        {
            GetComponent<Button>().interactable = false;
        }
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
        inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        // If there isnt an object then set the item's parent to the slot dropped on
        if (transform.childCount == 0)
        {
            Debug.Log(inventoryItem);
            inventoryItem.parentAfterDrag = transform;
        }
        // Otherwise swap positions between the items
        else
        {
            InventoryItem currentSlotItem = transform.GetComponentInChildren<InventoryItem>();
            currentSlotItem.gameObject.transform.SetParent(inventoryItem.parentAfterDrag);

            inventoryItem.parentAfterDrag.GetComponent<InventorySlot>().inventoryItem = currentSlotItem;

            inventoryItem.parentAfterDrag = transform;

            inventoryItem.parentAfterDrag.GetComponent<InventorySlot>().inventoryItem = inventoryItem;
        }
    }

    public void Clear()
    {
        inventoryItem = GetComponentInChildren<InventoryItem>();
        Destroy(inventoryItem.gameObject);
    }

    public void CraftButton()
    {
        if (ItemInSlot != null)
        {
            if (ItemInSlot.recipe != null)
            {
                if (crafting != null) 
                {
                    crafting.Craft(ItemInSlot.recipe);
                }
            }
        }
    }
}
