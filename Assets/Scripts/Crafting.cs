using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] InventoryManager inventory;

    public void Craft(CraftRecipe recipe)
    {
        // If there is free space
        if (!inventory.CheckFreeSpace())
        {
            Debug.Log("Not enough space to craft object");
            return;
        }

        for (int i = 0; i < recipe.elements.Count; i++)
        {
            if(inventory.CheckItem(recipe.elements[i]) == false)
            {
                Debug.Log("Not enough required elements to craft item");
                return;
            }
        }

        // Remove crafting components from inventory
        for (int i = 0; i < recipe.elements.Count; i++)
        {
            inventory.RemoveItem(recipe.elements[i].item, recipe.elements[i].inventoryItem.count);
        }

        // Add new crafted item
        for (int i = 0; i < recipe.output.count; i++)
        {
            inventory.AddItem(recipe.output.item);
        }
    }
}
