using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropTile 
{
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer renderer;
    public float damage;
    public Vector2Int position;

    public bool Complete
    {
        get 
        { 
            if(crop == null) { return false; }
            return growTimer >= crop.timeToGrow; 
        }
    }

    internal void Harvested()
    {
        growTimer = 0;
        growStage = 0;
        crop = null;
        renderer.gameObject.SetActive(false);
    }
}

public class CropsManager : TimeAgent
{
    public Player player;

    [SerializeField] TileBase plowed;
    [SerializeField] TileBase seeded;
    [SerializeField] TileBase tilled;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap parentTilemap;
    [SerializeField] UnityEngine.GameObject cropsSpritePrefab;

    Dictionary<Vector2Int, CropTile> crops;

    private void Start()
    {
        crops = new Dictionary<Vector2Int, CropTile>();
        onTimeTick += Tick;
        Init();
    }

    public void Tick()
    {
        foreach (CropTile cropTile in crops.Values)
        {
            if(cropTile.crop == null) { continue; }

            if (cropTile.Complete)
            {
                continue;
            }

            cropTile.growTimer += 1;

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }
        }
    }

    public bool Check(Vector3Int position)
    {
        return crops.ContainsKey((Vector2Int)position);
    }

    public void Plow(Vector3Int position)
    {
        // Checks if there is already crop in the position, otherwise create new crop
        if (crops.ContainsKey((Vector2Int)position))
        {
            return;
        }

        CreatePlowedTile(position);
    }

    public void Till(Vector3Int position)
    {
        parentTilemap.SetTile(new Vector3Int(position.x, position.y, 0), tilled);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        //targetTilemap.SetTile(position, seeded);

        crops[(Vector2Int)position].crop = toSeed;
    }

    public void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        Vector3 p = targetTilemap.CellToWorld(gridPosition);
        if (!crops.ContainsKey(position)) { return; }

        CropTile cropTile = crops[position];

        if (cropTile.Complete)
        {
            Instantiate(cropTile.crop.yield.obj, new Vector3(p.x, p.y, 0), cropTile.renderer.gameObject.transform.rotation);
            Debug.Log("Crop yielded");
            targetTilemap.SetTile(gridPosition, plowed);
            cropTile.Harvested();
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        crops.Add((Vector2Int)position, crop);

        // Creates a hidden gameobject on the plowed dirt that will render any crop sprites
        UnityEngine.GameObject go = Instantiate(cropsSpritePrefab);
        go.transform.position = targetTilemap.CellToWorld(position);
        go.SetActive(false);
        crop.renderer = go.GetComponent<SpriteRenderer>();

        targetTilemap.SetTile(position, plowed);
    }

}
