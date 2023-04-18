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
        damage = 0;
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
        FindCropsTilemap();
        crops = new Dictionary<Vector2Int, CropTile>();
        onTimeTick += Tick;
        Init();
    }

    public void FindCropsTilemap()
    {
        if (UnityEngine.GameObject.Find("Crops"))
        {
            parentTilemap = UnityEngine.GameObject.Find("Crops").GetComponent<Tilemap>();
        }
    }

    public void Tick()
    {
        foreach (CropTile cropTile in crops.Values)
        {
            if(cropTile.crop == null) { continue; }

            cropTile.damage += 0.02f;

            if(cropTile.damage > 1f)
            {
                cropTile.Harvested();
                continue;
            }

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

        // Checks for scene transition
        if(parentTilemap == null)
        {
            FindCropsTilemap();
        }
    }

    public bool Check(Vector3 position)
    {
        return crops.ContainsKey((Vector2Int)(new Vector3Int((int)position.x, (int)position.y, 0)));
    }

    public void Plow(Vector3 position)
    {
        // Checks if there is already crop in the position, otherwise create new crop
        if (crops.ContainsKey((Vector2Int)(new Vector3Int((int)position.x, (int)position.y, 0))))
        {
            return;
        }

        CreatePlowedTile(position);
    }

    public void Till(Vector3 position)
    {
        if(parentTilemap != null)
        {
            parentTilemap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), tilled);
        }
    }

    public void Seed(Vector3 position, Crop toSeed)
    {
        //targetTilemap.SetTile(position, seeded);

        crops[(Vector2Int)(new Vector3Int((int)position.x, (int)position.y, 0))].crop = toSeed;
    }

    public void PickUp(Vector3 gridPosition)
    {
        Vector2Int position = (Vector2Int)(new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0));
        Vector3 p = targetTilemap.LocalToWorld(gridPosition);
        if (!crops.ContainsKey(position)) { return; }

        CropTile cropTile = crops[position];

        if (cropTile.Complete)
        {
            Instantiate(cropTile.crop.yield.obj, new Vector3(p.x, p.y, 0), cropTile.renderer.gameObject.transform.rotation);
            Debug.Log("Crop yielded");
            targetTilemap.SetTile((new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0)), plowed);
            cropTile.Harvested();
        }
    }

    private void CreatePlowedTile(Vector3 position)
    {
        CropTile crop = new CropTile();
        crops.Add((Vector2Int)(new Vector3Int((int)position.x, (int)position.y, 0)), crop);

        // Creates a hidden gameobject on the plowed dirt that will render any crop sprites
        UnityEngine.GameObject go = Instantiate(cropsSpritePrefab);
        go.transform.position = targetTilemap.LocalToWorld(position);
        go.SetActive(false);
        crop.renderer = go.GetComponent<SpriteRenderer>();

        targetTilemap.SetTile((new Vector3Int((int)position.x, (int)position.y, 0)), plowed);
    }

}
