using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Recipe")]
public class CraftRecipe : ScriptableObject
{
    public List<InventorySlot> elements;
    public InventoryItem output;
    public ItemRecipe recipe;
}
