using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    public static MarkerManager instance;

    [SerializeField] Tilemap targetTilemap;
    [SerializeField] TileBase tile;
    public Vector3Int markedCellPosition;
    Vector3Int oldCellPosition;
    bool show = false;

    // Defaults marker to not shown
    private void Awake()
    {
        Show(false);
        instance = this;
    }

    private void Update()
    {
        if (show == false) { return; }
        targetTilemap.SetTile(oldCellPosition, null);
        targetTilemap.SetTile(markedCellPosition, tile);
        oldCellPosition = markedCellPosition;
    }

    public void Show(bool selectable)
    {
        show = selectable;
        targetTilemap.gameObject.SetActive(show);
    }
}
