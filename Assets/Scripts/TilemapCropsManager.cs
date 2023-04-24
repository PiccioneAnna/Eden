using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCropsManager : TimeAgent
{
    [SerializeField] UnityEngine.GameObject cropsSpritePrefab;

    [SerializeField] CropsContainer container;

    [SerializeField] TileBase plowed;
    [SerializeField] TileBase tilled;

    Tilemap targetTilemap;
    public Tilemap parentTilemap;
    private QuestManager questManager;

    private void Start()
    {
        GameManager.instance.GetComponent<CropsManager>().cropsManager = this; // Reporting this to this to prevent multi checks
        questManager = GameManager.instance.GetComponent<CropsManager>().questManager;
        targetTilemap = GetComponent<Tilemap>();
        onTimeTick += Tick;
        Init();
        VisualizeMap();
    }

    // Clean up crops in container upon destroy
    private void OnDestroy()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            container.crops[i].renderer = null;
        }
    }

    private void VisualizeMap()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            VisualizeTile(container.crops[i]);
        }
    }

    public void Tick()
    {
        if (targetTilemap == null) { return; }

        foreach (CropTile cropTile in container.crops)
        {
            if (cropTile.crop == null) { continue; }

            //cropTile.damage += 0.02f;

            // If crop ages beyond it's life then wither
            if (cropTile.damage > 1f)
            {
                cropTile.Harvested();
                continue;
            }

            // If the crop is ready to harvest don't do anything
            if (cropTile.Complete)
            {
                continue;
            }

            cropTile.growTimer += 1;
            // Update sprite growth based on time
            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                if(cropTile.growStage < cropTile.crop.growthStageTime.Count)
                {
                    cropTile.renderer.gameObject.SetActive(true);
                    cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                    cropTile.growStage += 1;
                }
            }
        }
    }

    internal bool Check(Vector3Int position)
    {
        return container.Get(position) != null;
    }

    public void Plow(Vector3Int position)
    {
        if(Check(position) == true) { return; }
        CreatePlowedTile(position);
        questManager.Hoe();
    }

    public void Till(Vector3Int position)
    {
        parentTilemap.SetTile(new Vector3Int(position.x, position.y, 0), tilled);
    }

    public void Seed(Vector3Int position, Crop toSeed)
    {
        CropTile tile = container.Get(position);

        if (tile == null) { return; }

        tile.crop = toSeed;
        questManager.Seed(toSeed);
        Tick();
    }

    public void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;
        Vector3 p = targetTilemap.CellToWorld(gridPosition);

        CropTile tile = container.Get(gridPosition);
        if(tile == null) { return; }

        if (tile.Complete)
        {
            Instantiate(tile.crop.yield.obj, new Vector3(p.x, p.y, 0), tile.renderer.gameObject.transform.rotation);
            Debug.Log("Crop yielded");
            questManager.Harvest(tile.crop);

            if (!tile.crop.multiHarvest)
            {
                tile.Harvested();
            }
            else
            {
                tile.Regrowth();
            }
            VisualizeTile(tile);
        }
    }

    public void VisualizeTile(CropTile cropTile)
    {
        targetTilemap.SetTile(new Vector3Int(cropTile.position.x, cropTile.position.y, 0), plowed);

        if (cropTile.renderer == null)
        {
            // Creates a hidden gameobject on the plowed dirt that will render any crop sprites
            UnityEngine.GameObject go = Instantiate(cropsSpritePrefab, transform);
            go.transform.position = targetTilemap.CellToWorld(new Vector3Int(cropTile.position.x, cropTile.position.y, 0));
            cropTile.renderer = go.GetComponent<SpriteRenderer>();
        }

        bool growing = cropTile.crop != null && cropTile.growTimer >= cropTile.crop.growthStageTime[0];

        if (growing)
        {
            cropTile.renderer.gameObject.SetActive(true);
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage-1];
        }
    }

    private void CreatePlowedTile(Vector3Int position)
    {
        CropTile crop = new CropTile();
        container.Add(crop);

        crop.position = (Vector2Int)position;

        VisualizeTile(crop);

        targetTilemap.SetTile(position, plowed);
    }
}