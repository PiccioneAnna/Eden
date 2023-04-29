using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConvertorData
{
    public InventorySlot inventorySlot;
    public float timer;

    public ItemConvertorData()
    {
        inventorySlot = new InventorySlot();
    }
}

public class ItemConverterInteract : Interactable
{
    [SerializeField] Item convertableItem;
    [SerializeField] Item producedItem;
    [SerializeField] int producedItemCount = 1;

    ItemConvertorData data;

    [SerializeField] float timeToProcess = 5f;

    private void Start()
    {
        data = new ItemConvertorData();
    }

    public override void Interact(Player player)
    {
        // If item put in the GUi is convertible, start processing it
    }
}
