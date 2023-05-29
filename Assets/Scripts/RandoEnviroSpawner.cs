using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandoEnviroSpawner : MonoBehaviour
{

    public RandoEnviroSpawner instance;

    public int minSpawnCount;
    public int maxSpawnCount;
    public int spawnCount;

    public EnviroSpawner objects;

    public PolygonCollider2D spawnArea;
    public BoxCollider2D spawnBounds;

    private Vector2 position;
    private Quaternion rotation;
    private Vector3 scale;

    private System.Random random;

    Bounds colliderBounds;
    Vector3 colliderCenter;
    float[] ranges;

    // Start is called before the first frame update
    void Start()
    {
        colliderBounds = spawnArea.bounds;
        colliderCenter = colliderBounds.center;

        ranges = new float[]{
            colliderCenter.x - colliderBounds.extents.x,
            colliderCenter.x + colliderBounds.extents.x,
            colliderCenter.y - colliderBounds.extents.y,
            colliderCenter.y + colliderBounds.extents.y,
        };

        instance = this;
        position = transform.position;
        rotation = transform.rotation;

        // Random Spawn Count
        random = new System.Random();
        spawnCount = random.Next(maxSpawnCount) + minSpawnCount;

        // Divide up spawncoutn by object rarity;
        // For the amount of items, spawn
        for (int j = 0; j < (spawnCount * objects.commonPercentage); j++)
        {
            if (objects.common.Count == 0) { return; }
            CreateObject(RandomObjectPrefab(objects.common));
        }
        for (int k = 0; k < (spawnCount * objects.uncommonPercentage); k++)
        {
            if (objects.uncommon.Count == 0) { return; }
            CreateObject(RandomObjectPrefab(objects.uncommon));
        }
        for (int l = 0; l < (spawnCount * objects.rarePercentage); l++)
        {
            if (objects.rare.Count == 0) { return; }
            CreateObject(RandomObjectPrefab(objects.rare));
        }
    }

    private void RandomPosition()
    {
        float randomX = Random.Range(ranges[0], ranges[1]);
        float randomY = Random.Range(ranges[2], ranges[3]);

        position = new Vector2(randomX, randomY);
    }

    // Get a random object to spawn;
    private GameObject RandomObjectPrefab(List<GameObject> objects)
    {
        return objects[random.Next(objects.Count)];
    }

    private bool RandomSign()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return true;
        }
        return false;
    }

    // Creates object instance
    private void CreateObject(GameObject drop)
    {
        // Randomized scale
        Resource resource = drop.GetComponent<ResourceNode>().resource;
        float s = UnityEngine.Random.Range(resource.minScale, resource.maxScale);
        scale = new Vector3(s, s, 0);
        RandomPosition();

        // Checks if object is trying to spawn somewhere it shouldn't
        if (!spawnArea.OverlapPoint(position))
        {
            // While object is spawned in non spawn area or outside of map, find new position
            while (!spawnArea.OverlapPoint(position))
            {
                RandomPosition();
            }
        }

        GameObject go = Instantiate(drop, new Vector3(position.x, position.y, 0) + transform.position, rotation, this.transform);
        go.gameObject.transform.localScale = scale;

        if (go.gameObject.GetComponent<BoxCollider2D>() != null)
        {
            Vector3 size = go.gameObject.GetComponent<BoxCollider2D>().size;
            go.gameObject.GetComponent<BoxCollider2D>().size = new Vector3(size.x * scale.x, size.y * scale.y, 0);
        }

        bool isFlip = RandomSign();

        go.gameObject.GetComponent<SpriteRenderer>().flipX = isFlip;

    }
}
