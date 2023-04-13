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

            cropTile.growTimer += 1;

            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

                cropTile.growStage += 1;
            }

            if (cropTile.growTimer >= cropTile.crop.timeToGrow)
            {
                cropTile.crop = null;
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
