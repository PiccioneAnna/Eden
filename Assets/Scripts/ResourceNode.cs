using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : ToolHit
{
    public Resource resource;
    private int health;
    // Randomized drops
    private UnityEngine.GameObject drop;
    UnityEngine.GameObject[] droppedObjs;
    private System.Random random;
    private Vector3 position;
    private Quaternion rotation;
    private float offsetX, offsetY;
    private int multplierX, multplierY;
    private int maxDropCount, minDropCount, dropCount;
    [SerializeField] ResourceNodeType nodeType;

    private void Awake()
    {
        random = new System.Random();
        position = transform.position;
        rotation = transform.rotation;
        maxDropCount = resource.maxDropCount;
        minDropCount = resource.minDropCount;
        dropCount = random.Next(maxDropCount) + minDropCount + (int)transform.localScale.x;
        droppedObjs = resource.droppedObjs;
        health = resource.health;
    }

    public override void Hit()
    {
        health--;
        resource.Shake();
        if (!resource.multisprite)
        {
            resource.HitTree(health);
        }
        Debug.Log(health);

        if (health <= 0)
        {
            Debug.Log("Drop Count:" + dropCount);
            for (int i = 0; i < dropCount; i++)
            {
                random = new System.Random();
                drop = droppedObjs[random.Next(droppedObjs.Length)];

                // Randomized drop positoning
                offsetX = (float)random.NextDouble() / 4;
                offsetY = (float)random.NextDouble() / 8;
                multplierX = offsetX % 2 == 2 ? 1 : -1;
                multplierY = offsetY % 2 == 2 ? 1 : -1;

                // Randomized drop
                position = new Vector3(position.x + (multplierX * offsetX), position.y + (multplierY * offsetY), position.z);
                Instantiate(drop, position, rotation);
            }
            Destroy(gameObject);
        }
    }

    public override bool CanBeHit(List<ResourceNodeType> canBeHit)
    {
        return canBeHit.Contains(nodeType);
    }
}
