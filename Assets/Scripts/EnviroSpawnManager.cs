using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroSpawnManager : MonoBehaviour
{
    public static EnviroSpawnManager instance;

    public int minSpawnCount;
    public int maxSpawnCount;
    public int spawnCount;

    public List<UnityEngine.GameObject> objects;

    public SpriteRenderer outerSpawnArea;
    public SpriteRenderer innerSpawnArea;

    public PolygonCollider2D collider2D;
    public PolygonCollider2D spawnArea;

    private float outerRadius;
    private float innerRadius;

    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    private System.Random random;

    void Awake()
    {
        instance = this;
        position = transform.position;
        rotation = transform.rotation;

        outerRadius = outerSpawnArea.bounds.size.x/2;
        innerRadius = innerSpawnArea.bounds.size.x/2;

        // Random Spawn Count
        random = new System.Random();
        spawnCount = random.Next(maxSpawnCount) + minSpawnCount;

        // For the amount of items, spawn
        for (int i = 0; i < spawnCount; i++)
        {
            CreateObject(RandomObjectPrefab());
        }
    }

    private void RandomPosition()
    {
        // Randomized point within a donut
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio * ratio, 1f)) * outerRadius;
        Vector3 point = Random.insideUnitCircle.normalized * radius;

        position = new Vector3((int)point.x,(int)point.y,0);
    }

    // Get a random object to spawn;
    private UnityEngine.GameObject RandomObjectPrefab()
    {
        return objects[random.Next(objects.Count)];
    }

    // Creates object instance
    private void CreateObject(UnityEngine.GameObject drop)
    {
        // Randomized scale
        Resource resource = drop.GetComponent<ResourceNode>().resource;
        float s = Random.Range(resource.minScale, resource.maxScale);
        scale = new Vector3(s, s, 0);
        RandomPosition();

        // Checks if object is trying to spawn somewhere it shouldn't
        if (collider2D.OverlapPoint(new Vector2(position.x, position.y)) ||
            !spawnArea.OverlapPoint(new Vector2(position.x, position.y)))
        {
            // While object is spawned in non spawn area or outside of map, find new position
            Debug.Log("Collision detected in spawning object, fixing point");
            while (collider2D.OverlapPoint(new Vector2(position.x, position.y)) ||
                !spawnArea.OverlapPoint(new Vector2(position.x, position.y)) )
            {
                RandomPosition();
            }
        }

        UnityEngine.GameObject go = Instantiate(drop, position + transform.position, rotation, this.transform);
        go.gameObject.transform.localScale = scale;

        go.gameObject.GetComponent<SpriteRenderer>().flipX = RandomSign();
    }

    private bool RandomSign()
    {
        if (Random.Range(0, 2) == 0)
        {
            return true;
        }
        return false;
    }
}
