using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoal : Quest.QuestGoal
{
    private InventoryManager inventoryManager;
    public Item item;
    private int count;

    public override string GetDescription()
    {
        return "Obtain" + count + " " + item.name;
    }

    public override void Initialize()
    {
        base.Initialize();
        inventoryManager = GameManager.instance.GetComponent<InventoryManager>();
        EventManager.Instance.AddListener<ItemGameEvent>(OnItem);
    }

    private void OnItem(ItemGameEvent ge)
    {
        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null && itemInSlot.item == item)
            {
                count = inventoryManager.CheckItemCount(item);
            }
        }

        if (count > 0)
        {
            CurrentAmount = count;
            Evaluate();
        }
    }
}
