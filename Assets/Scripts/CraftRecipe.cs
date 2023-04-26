using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecipeElement
{
    public Item item;
    public int count;
}

// Serialzaqble field did not show up in inspector so repreated myuself here
[CreateAssetMenu(menuName = "Data/Recipe")]
public class CraftRecipe : ScriptableObject
{
    public List<RecipeElement> inputs;
    public Item output;
}
