using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] TileBase tile;
    public Vector3 markedCellPosition;
    Vector3 oldCellPosition;
    bool show = false;

    // Defaults marker to not shown
    private void Awake()
    {
        Show(false);
    }

    private void Update()
    {
        if (show == false) { return; }
        targetTilemap.SetTile(new Vector3Int((int)oldCellPosition.x, (int)oldCellPosition.y, 0), null);
        Debug.Log((new Vector3Int((int)markedCellPosition.x, (int)markedCellPosition.y, 0)));
        targetTilemap.SetTile(new Vector3Int((int)markedCellPosition.x, (int)markedCellPosition.y, 0), tile);
        oldCellPosition = markedCellPosition;
    }

    public void Show(bool selectable)
    {
        show = selectable;
        targetTilemap.gameObject.SetActive(show);
    }
}
