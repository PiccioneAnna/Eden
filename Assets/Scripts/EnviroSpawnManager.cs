using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnviroSpawner
{
    public List<GameObject> common;
    public List<GameObject> uncommon;
    public List<GameObject> rare;

    public float commonPercentage;
    public float uncommonPercentage;
    public float rarePercentage;

    public List<GameObject> spawnedObjects;
    public List<Vector3> spawnedObjectsPositions;
    public List<Vector3> spawnedObjectsScale;
    public List<bool> spawnedObjectsFlipX;

    public EnviroSpawner()
    {
        spawnedObjects = new List<GameObject>();
        spawnedObjectsPositions = new List<Vector3>();
        spawnedObjectsFlipX = new List<bool>();
        spawnedObjectsScale = new List<Vector3>();
    }
}

public class EnviroSpawnManager : MonoBehaviour, IPersistent
{
    public EnviroSpawnManager instance;

    public int minSpawnCount;
    public int maxSpawnCount;
    public int spawnCount;

    public EnviroSpawner objects;
    private static EnviroSpawner data;

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

    void Start()
    {
        Debug.Log(data);

        if (data == null)
        {
            data = new EnviroSpawner();

            Debug.Log(data);

            instance = this;
            position = transform.position;
            rotation = transform.rotation;

            outerRadius = outerSpawnArea.bounds.size.x / 2;
            innerRadius = innerSpawnArea.bounds.size.x / 2;

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
        else
        {
            Debug.Log("Recreating enviro objects");
            Debug.Log(data.spawnedObjects.Count);
            RecreateObjects();
        }
    }

    private void RandomPosition()
    {
        // Randomized point within a donut
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(UnityEngine.Random.Range(ratio * ratio, 1f)) * outerRadius;
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * radius;

        position = new Vector3((int)point.x,(int)point.y,0);
    }

    // Get a random object to spawn;
    private GameObject RandomObjectPrefab(List<GameObject> objects)
    {
        return objects[random.Next(objects.Count)];
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
        if (collider2D.OverlapPoint(new Vector2(position.x, position.y)) ||
            !spawnArea.OverlapPoint(new Vector2(position.x, position.y)))
        {
            // While object is spawned in non spawn area or outside of map, find new position
            while (collider2D.OverlapPoint(new Vector2(position.x, position.y)) ||
                !spawnArea.OverlapPoint(new Vector2(position.x, position.y)) )
            {
                RandomPosition();
            }
        }

        GameObject go = Instantiate(drop, position + transform.position, rotation, this.transform);
        go.gameObject.transform.localScale = scale;

        if(go.gameObject.GetComponent<BoxCollider2D>() != null)
        {
            Vector3 size = go.gameObject.GetComponent<BoxCollider2D>().size;
            go.gameObject.GetComponent<BoxCollider2D>().size = new Vector3(size.x * scale.x, size.y * scale.y, 0);
        }

        bool isFlip = RandomSign();

        go.gameObject.GetComponent<SpriteRenderer>().flipX = isFlip;

        data.spawnedObjects.Add(drop);
        data.spawnedObjectsPositions.Add(position + transform.position);
        data.spawnedObjectsFlipX.Add(isFlip);
        data.spawnedObjectsScale.Add(scale);
    }

    // If the game is already loaded then take the objects that have already been spawned and recreate them
    // Saves type of object, position, scale, flip x
    private void RecreateObjects()
    {
        for (int i = 0; i < data.spawnedObjects.Count; i++)
        {
            GameObject go = Instantiate(data.spawnedObjects[i], data.spawnedObjectsPositions[i], rotation, this.transform);
            go.gameObject.transform.localScale = data.spawnedObjectsScale[i];

            if (go.gameObject.GetComponent<BoxCollider2D>() != null)
            {
                Vector3 size = go.gameObject.GetComponent<BoxCollider2D>().size;
                go.gameObject.GetComponent<BoxCollider2D>().size = new Vector3(size.x * data.spawnedObjectsScale[i].x, size.y * data.spawnedObjectsScale[i].y, 0);
            }

            go.gameObject.GetComponent<SpriteRenderer>().flipX = data.spawnedObjectsFlipX[i];
        }
    }

    private bool RandomSign()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return true;
        }
        return false;
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<EnviroSpawner>(jsonString);
    }
}
