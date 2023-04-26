using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager craftingManager;

    public Crafting crafting;

    public List<CraftRecipe> knownRecipes;
    public Player player;
    public InventorySlot[] inventorySlots;
    public UnityEngine.GameObject inventoryItemPrefab;

    void Awake()
    {
        craftingManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (CraftRecipe recipe in knownRecipes)
        {
            AddItem(recipe.output);
        }
    }

    public bool AddItem(Item item)
    {
        // Find any empty slot
        for (int i = 0; i < knownRecipes.Count; i++)
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

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        UnityEngine.GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
