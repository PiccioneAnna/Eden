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

        for (int i = 0; i < recipe.inputs.Count; i++)
        {
            if(inventory.CheckItem(recipe.inputs[i]) == false)
            {
                Debug.Log("Not enough required elements to craft item");
                return;
            }
        }

        // Remove crafting components from inventory
        for (int i = 0; i < recipe.inputs.Count; i++)
        {
            inventory.RemoveItem(recipe.inputs[i].item, recipe.inputs[i].count);
        }

        // Add new crafted item
        inventory.AddItem(recipe.output);
    }
}
