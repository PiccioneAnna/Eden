using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager;

    public InventorySlot[] inventorySlots;
    public UnityEngine.GameObject inventoryItemPrefab;
    public int maxCount = 999;
    public int toolbarCount = 6;
    public Item selectedItem;

    int selectedSlot = -1;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (inventoryManager == null)
        {
            inventoryManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        // Checks if user is pressing keys for toolbar switch
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= toolbarCount)
            {
                ChangeSelectedSlot(number - 1);
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    if(selectedSlot < toolbarCount && selectedSlot >= 1)
                    {
                        ChangeSelectedSlot(selectedSlot - 1); 
                    }
                    else
                    {
                        ChangeSelectedSlot(toolbarCount-1);
                    }
                }
                if (Input.mouseScrollDelta.y < 0)
                {
                    if (selectedSlot < toolbarCount-1)
                    {
                        ChangeSelectedSlot(selectedSlot + 1); 
                    }
                    else
                    {
                        ChangeSelectedSlot(0);
                    }
                }
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        // Selects the item the player picked in the toolbar, checks if slot is empty first
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        if (inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() != null)
        {
            selectedItem = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item;
            Debug.Log("Selected Item: " + selectedItem.itemName);
        }
        else
        {
            selectedItem = null;
            Debug.Log("Selected Item: Null");
        }
    }

    public bool AddItem(Item item)
    {
        // Check if any slot has the same item with count lower then the max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && 
                itemInSlot.item == item && 
                itemInSlot.count < maxCount && 
                itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                slot.ItemInSlot = item;
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item, int count = 1)
    {
        if (item.stackable)
        {
            InventorySlot inventorySlot = null;
            foreach (InventorySlot slot in inventorySlots)
            {
                if(slot.ItemInSlot == item)
                {
                    inventorySlot = slot;
                }
            }
            if (inventorySlot == null) { return; }

            inventorySlot.GetComponent<InventoryItem>().count -= count;
            if(inventorySlot.GetComponent<InventoryItem>().count < 0)
            {
                inventorySlot.Clear();
            }
        }
        else
        {
            while(count > 0)
            {
                count -= 1;

                InventorySlot inventorySlot = null;
                foreach (InventorySlot slot in inventorySlots)
                {
                    if (slot.ItemInSlot == item)
                    {
                        inventorySlot = slot;
                    }
                }
                if (inventorySlot == null) { return; }

                inventorySlot.Clear();
            }
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        UnityEngine.GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
           Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
        }

        return null;
    }

}
