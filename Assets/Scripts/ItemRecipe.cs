using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item Recipe")]
public class ItemRecipe : ScriptableObject
{
    [System.Serializable]
    public struct inputElement
    {
        public Item item;
        public int count;
    }

    public inputElement[] inputs;
    public Item output;
}
