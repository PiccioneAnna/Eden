using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlaceableObject
{
    public Item placedItem;
    public Transform targetObject;
    public Vector3Int positionOnGrid;
    public string objectState; // this is a serialized JSON string

    public PlaceableObject(Item item, Vector3Int pos)
    {
        placedItem = item;
        positionOnGrid = pos;
    }
}

[CreateAssetMenu(menuName = "Data/Placeable Object Container")]
public class PlaceableObjectsContainer : ScriptableObject
{
    public List<PlaceableObject> placeableObjects;

    internal PlaceableObject Get(Vector3Int position)
    {
        foreach (PlaceableObject obj in placeableObjects)
        {
            if(obj.positionOnGrid == position)
            {
                return obj;
            }
        }
        return null;
    }

    internal void Remove(PlaceableObject placedObject)
    {
        placeableObjects.Remove(placedObject);
    } 
}
