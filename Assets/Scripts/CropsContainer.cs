using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crops Container")]
public class CropsContainer : ScriptableObject
{
    public List<CropTile> crops;

    public CropTile Get(Vector3Int position)
    {
        return crops.Find(x => x.position == (Vector2Int)position);
    }

    public void Add(CropTile crop)
    {
        crops.Add(crop);
    }

    public void Clear()
    {
        crops.Clear();
    }
}
